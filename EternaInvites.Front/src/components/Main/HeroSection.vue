<template>
  <section class="hero-section">
    <div class="hero-content animate-fade-in">
      <h1 class="text-h2 text-white q-mb-md">{{ $t('main.heroTitle') }}</h1>
      <h2 class="text-h5 text-white q-mb-xl">{{ $t('main.heroSubtitle') }}</h2>
      <div class="search-container">
        <q-input
          v-model="searchName"
          outlined
          bg-color="white"
          class="search-input rounded-input"
          :placeholder="$t('main.searchPlaceholder')"
          @focus="searchFocused = true"
          @blur="searchFocused = !searchName"
          :class="{ 'focused-input': searchFocused }">
          <template v-slot:prepend>
            <q-icon name="search" color="primary" />
          </template>
          <template v-slot:append>
            <q-btn
              round
              flat
              dense
              icon="arrow_forward"
              @click="handleSearch"
              color="primary"
              class="search-btn" />
          </template>
        </q-input>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
  import { ref } from 'vue'
  import { useRouter } from 'vue-router'

  const router = useRouter()
  const searchName = ref('')
  const searchFocused = ref(false)

  const handleSearch = () => {
    if (searchName.value.trim()) {
      router.push({
        path: '/' + searchName.value,
      })
    }
  }
</script>

<style scoped>
  /* Hero Section */
  .hero-section {
    height: 100vh;
    background-image:
      linear-gradient(rgba(0, 0, 0, 0.4), rgba(0, 0, 0, 0.4)),
      url('https://images.unsplash.com/photo-1519741497674-611481863552?ixlib=rb-1.2.1&auto=format&fit=crop&w=1350&q=80');
    background-size: cover;
    background-position: center;
    display: flex;
    align-items: center;
    justify-content: center;
    text-align: center;
    color: white;
  }

  .hero-content {
    max-width: 800px;
    padding: 0 20px;
  }

  /* Search Input Styling */
  .search-container {
    max-width: 600px;
    margin: 0 auto;
  }

  .search-input {
    border-radius: 50px !important;
    box-shadow: 0 4px 16px rgba(0, 0, 0, 0.25);
    transition: all 0.3s ease;
  }

  .search-input .q-field__control {
    border-radius: 50px;
    height: 58px;
    background: rgba(255, 255, 255, 0.95);
  }

  .search-input .q-field__append {
    padding-right: 8px;
  }

  .search-input .q-field__prepend {
    padding-left: 16px;
  }

  .search-btn {
    transition: transform 0.3s ease;
  }

  .search-btn:hover {
    transform: translateX(3px);
  }

  .focused-input {
    transform: scale(1.05);
    box-shadow: 0 6px 20px rgba(0, 0, 0, 0.35);
  }
</style>
