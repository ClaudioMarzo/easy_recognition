import os
import requests
from django.http import JsonResponse
from django.views.decorators.csrf import csrf_exempt

API_KEY = os.getenv('API_KEY')
API_SECRET = os.getenv('API_SECRET')

@csrf_exempt
def recognize_face(request):
    if request.method == 'POST':
        image_data = request.FILES.get('image')
        if not image_data:
            return JsonResponse({'error': 'Nenhuma imagem fornecida'}, status=400)

        files = {'image_file': image_data}
        data = {
            'api_key': API_KEY,
            'api_secret': API_SECRET,
            # Outros parâmetros conforme a documentação da Face++
        }

        response = requests.post('https://api-us.faceplusplus.com/facepp/v3/search', data=data, files=files)
        result = response.json()

        if 'faces' in result and len(result['faces']) > 0:
            # Processar o resultado e retornar o nome do usuário
            return JsonResponse({'name': 'Usuário Reconhecido'})
        else:
            return JsonResponse({'error': 'Rosto não reconhecido ou iluminação insuficiente'}, status=400)
    else:
        return JsonResponse({'error': 'Método não permitido'}, status=405)
