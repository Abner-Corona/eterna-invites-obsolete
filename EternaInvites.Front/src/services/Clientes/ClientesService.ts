import { api } from 'src/boot/axios'
import { ObtenerInivitacionResponse } from './ObtenerInivitacion'

// Define interfaces that match the backend models
export interface ClientesModel {
  id?: number
  uui?: string
  nombre: string
  apellido: string
  apellidoPaterno: string
  url: string
  correo: string
  telefono: string
  direccion: string
  fechaEvento: string
  descripcion: string
  nombreEvento: string
}

const endpoint = 'Clientes'

export const ClientesService = {
  async Insert(request: ClientesModel): Promise<ClientesModel> {
    const response = await api.post<ClientesModel>(endpoint, request)
    return response.data
  },

  async GetAll(): Promise<ClientesModel[]> {
    const response = await api.get<ClientesModel[]>(endpoint)
    return response.data
  },

  async GetById(id: number): Promise<ClientesModel> {
    const response = await api.get<ClientesModel>(`${endpoint}/${id}`)
    return response.data
  },

  async Update(request: ClientesModel): Promise<ClientesModel> {
    const response = await api.put<ClientesModel>(`${endpoint}/${request.id}`, request)
    return response.data
  },

  async Delete(id: number): Promise<void> {
    await api.delete(`${endpoint}/${id}`)
  },

  async ObtenerInivitacion(url: string) {
    const response = await api.get<ObtenerInivitacionResponse>(
      `${endpoint}/ObtenerInivitacion/${url}`,
    )
    return response.data
  },
}
