import { api } from 'src/boot/axios'

// Define interfaces that match the backend models
export interface PlantillasModel {
  id: number
  uui: string
  nombre: string
  mostrarDemo: boolean
  html: string
  componentes: Componente[]
  categoria: string
}

export interface Componente {
  id: string
  nombre: string
  tipo: string
  contenido: string
  propiedades: Propiedades
  clases: string[]
  estilos: Estilo[]
  hijos: string[]
  eventos: Eventos
}

export interface Propiedades {
  additionalProp1: string
  additionalProp2: string
  additionalProp3: string
}

export interface Estilo {
  selector: string
}

export interface Eventos {
  additionalProp1: string
  additionalProp2: string
  additionalProp3: string
}

const endpoint = 'Plantillas'

export const PlantillasService = {
  async Insert(request: PlantillasModel): Promise<PlantillasModel> {
    const response = await api.post<PlantillasModel>(endpoint, request)
    return response.data
  },

  async GetAll(): Promise<PlantillasModel[]> {
    const response = await api.get<PlantillasModel[]>(endpoint)
    return response.data
  },

  async GetById(id: number): Promise<PlantillasModel> {
    const response = await api.get<PlantillasModel>(`${endpoint}/${id}`)
    return response.data
  },

  async Update(request: PlantillasModel): Promise<PlantillasModel> {
    const response = await api.put<PlantillasModel>(`${endpoint}/${request.id}`, request)
    return response.data
  },

  async Delete(id: number): Promise<void> {
    await api.delete(`${endpoint}/${id}`)
  },
}
