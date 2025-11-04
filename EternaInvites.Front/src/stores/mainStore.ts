import { defineStore, acceptHMRUpdate } from 'pinia'

interface MainStoreState {
  mobileMenuOpen: boolean
  isLoading: boolean
  currentRoute: string
  isScrolled: boolean
}

export const useMainStore = defineStore('main', {
  state: (): MainStoreState => ({
    mobileMenuOpen: false,
    isLoading: false,
    currentRoute: '/',
    isScrolled: false,
  }),

  getters: {
    isMobileMenuOpen: (state) => state.mobileMenuOpen,
    getLoadingState: (state) => state.isLoading,
    getCurrentRoute: (state) => state.currentRoute,
    getIsScrolled: (state) => state.isScrolled,
  },

  actions: {
    toggleMobileMenu() {
      this.mobileMenuOpen = !this.mobileMenuOpen
    },

    closeMobileMenu() {
      this.mobileMenuOpen = false
    },

    openMobileMenu() {
      this.mobileMenuOpen = true
    },

    setLoading(loading: boolean) {
      this.isLoading = loading
    },

    setCurrentRoute(route: string) {
      this.currentRoute = route
      // Close mobile menu when navigating
      this.closeMobileMenu()
    },

    setScrolled(scrolled: boolean) {
      this.isScrolled = scrolled
    },

    handleScroll(scrollTop: number) {
      const threshold = 30 // Trigger transition after 30px scroll
      this.setScrolled(scrollTop > threshold)
    },

    handleNavigation(path: string) {
      this.setCurrentRoute(path)
      console.log(`Navigated to: ${path}`)
    },
  },

  persist: false,
})

if (import.meta.hot) {
  import.meta.hot.accept(acceptHMRUpdate(useMainStore, import.meta.hot))
}
