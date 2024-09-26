from django.urls import path
from . import views

urlpatterns = [
    path('identify/', views.identify_face, name='identify_face'),
]
