import { api } from 'src/boot/axios'

// Define interfaces that match the backend models
export interface LoginRequest {
  usuario: string
  contrasena: string
}

export interface LoginResponse {
  id: number
  uuid: string
  token: string
}

export const LoginService = {
  async Login(request: LoginRequest) {
    const response = await api.post<LoginResponse>('Login', request)
    return response.data
  },
  async Test() {
    const response = await api.get('Login')
    return response.data
  },
}
