namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class ImagemController : ControllerBase
{
    /// <summary>
    /// Rota para comparar faces em uma imagem fornecida.
    /// </summary>
    [HttpPost("compare-image")]
    public async Task<IActionResult> CompareFaces([FromForm] ImagemModel imagemModel)
    {
        // Valida se a imagem foi recebida
        if (imagemModel.Imagem != null && imagemModel.Imagem.Length > 0)
        {
            // Salva a imagem recebida localmente com um nome baseado no timestamp atual
            string nomeDaImagem = $"{DateTime.Now:yyyyMMdd_HHmmss}.jpg";
            var imagePath = Path.Combine("reconhecimento_teste", nomeDaImagem); // **Caminho relativo**
            
            // Salva a imagem no diretório definido
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                imagemModel.Imagem.CopyTo(stream);
            }

            // Carrega a imagem para obter dimensões e validações
            using (var img = System.Drawing.Image.FromFile(imagePath))
            {
                Console.WriteLine($"Largura: {img.Width}, Altura: {img.Height}");
            }

            string modelsDirectory = Path.Combine(Environment.CurrentDirectory, "models");

            // Usa biblioteca FaceRecognition para processar a imagem
            using (var faceRecognition = FaceRecognition.Create(modelsDirectory))
            using (var unknownImage = FaceRecognition.LoadImageFile(imagePath))
            {
                var faceLocations = faceRecognition.FaceLocations(unknownImage);

                // Ordena as faces detectadas pela área (maior face primeiro)
                var mainFace = faceLocations
                    .OrderByDescending(rect => (rect.Right - rect.Left) * (rect.Bottom - rect.Top))
                    .FirstOrDefault();

                // Valida se uma face foi encontrada
                if (mainFace == null)
                {
                    return BadRequest("Nenhuma face encontrada na imagem.");
                }

                // Verifica se a face é grande o suficiente para a análise
                int width = mainFace.Right - mainFace.Left;
                int height = mainFace.Bottom - mainFace.Top;
                if (width < 150 || height < 150)
                {
                    return BadRequest("Muito longe, aproxime mais o rosto!");
                }

                // Realiza a codificação da face para comparação
                List<Location> mainFaceList = new() { mainFace };
                var unknownEncoding = faceRecognition.FaceEncodings(unknownImage, mainFaceList).FirstOrDefault();

                // Cria a requisição JSON para enviar ao servidor KNN
                var requestBody = new
                {
                    knn = new
                    {
                        field = "face_embeding",
                        query_vector = unknownEncoding.GetRawEncoding(),
                        k = 3,
                        num_candidates = 10
                    },
                    _source = new[] { "name", "position" }
                };

                var requestJson = JsonSerializer.Serialize(requestBody);

                try
                {
                    using var httpClient = new HttpClient();
                    var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

                    // Envia a requisição para o servidor Elasticsearch
                    var response = await httpClient.PostAsync("http://localhost:9200/faces/_knn_search", content);

                    // Processa a resposta do servidor
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var responseObject = JsonSerializer.Deserialize<ResponseObject>(responseContent);

                        if (responseObject?.hits?.hits != null)
                        {
                            // Seleciona informações importantes da resposta
                            var hits = responseObject.hits.hits.Select(hit => new
                            {
                                hit._source?.name,
                                score = hit._score,
                                hit._source?.position
                            }).ToList();

                            var importantInfo = new
                            {
                                responseObject.hits.max_score,
                                hits
                            };

                            return Ok(JsonSerializer.Serialize(importantInfo));
                        }
                        else
                        {
                            return BadRequest("Erro ao processar a resposta.");
                        }
                    }
                    else
                    {
                        return BadRequest("Erro ao conectar com o servidor.");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest($"Erro de conexão: {ex.Message}");
                }
            }
        }
        else
        {
            return BadRequest("Nenhuma imagem recebida.");
        }
    }

    /// <summary>
    /// Rota para salvar a imagem no servidor com nome e posição.
    /// </summary>
    [HttpPost("save-image")]
    public async Task<IActionResult> SalvarImagemAsync([FromForm] ImagemModel imagemModel, [FromForm] string nome)
    {
        if (imagemModel.Imagem != null && imagemModel.Imagem.Length > 0)
        {
            string nomeDaImagem = $"{DateTime.Now:yyyyMMdd_HHmmss}.jpg";
            var imagePath = Path.Combine("reconhecimento_teste", nomeDaImagem); // **Caminho relativo**

            // Salva a imagem recebida localmente
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                imagemModel.Imagem.CopyTo(stream);
            }

            string modelsDirectory = Path.Combine(Environment.CurrentDirectory, "models");

            // Processa a imagem para obter face embeding
            using (var faceRecognition = FaceRecognition.Create(modelsDirectory))
            using (var unknownImage = FaceRecognition.LoadImageFile(imagePath))
            {
                var faceLocations = faceRecognition.FaceLocations(unknownImage);
                var mainFace = faceLocations
                    .OrderByDescending(rect => (rect.Right - rect.Left) * (rect.Bottom - rect.Top))
                    .FirstOrDefault();

                if (mainFace == null)
                {
                    return BadRequest("Nenhuma face encontrada na imagem.");
                }

                var mainFaceList = new List<Location>() { mainFace };
                var unknownEncoding = faceRecognition.FaceEncodings(unknownImage, mainFaceList).FirstOrDefault();

                // Cria a requisição JSON para enviar ao servidor
                var requestBody = new
                {
                    name = nome,
                    face_embeding = unknownEncoding?.GetRawEncoding(),
                    position = "frontal"
                };

                var requestJson = JsonSerializer.Serialize(requestBody);

                try
                {
                    using var httpClient = new HttpClient();
                    var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

                    // Envia a requisição para o servidor Elasticsearch
                    var response = await httpClient.PostAsync("http://localhost:9200/faces/_doc/", content);

                    if (!response.IsSuccessStatusCode)
                    {
                        return BadRequest("Imagem não salva.");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest($"Erro de conexão: {ex.Message}");
                }
            }

            return Ok("Imagem recebida e salva com sucesso!");
        }
        else
        {
            return BadRequest("Nenhuma imagem recebida.");
        }
    }
}
