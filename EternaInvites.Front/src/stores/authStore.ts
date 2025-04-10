import { defineStore, acceptHMRUpdate } from 'pinia'
class AuthStoreStoreState {
  token?: string
}
export const useAuthStore = defineStore('authStore', {
  state: (): AuthStoreStoreState => ({
    token: '',
  }),

  getters: {},

  actions: {},
  persist: true,
})

if (import.meta.hot) {
  import.meta.hot.accept(acceptHMRUpdate(useAuthStore, import.meta.hot))
}
