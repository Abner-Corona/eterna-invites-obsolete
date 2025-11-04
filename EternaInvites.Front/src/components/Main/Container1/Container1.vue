<template>
  <div class="container-1">
    <div class="row full-width q-pa-lg">
      <!-- Left side - Card Images Grid -->
      <div class="col-xs-12 col-md-6 flex flex-center q-pa-md left-side-container">
        <div
          class="stacked-images-container"
          v-motion
          :initial="{ opacity: 0, y: -30 }"
          :enter="{ opacity: 1, y: 0 }"
          :duration="800"
          :delay="200">
          <q-img
            v-for="(card, index) in cardImages"
            :key="`card-${index}`"
            class="invitation-card card-item stacked-image"
            :src="card.src"
            :alt="card.alt"
            v-motion
            :style="{ zIndex: cardImages.length - index }"
            :initial="{ opacity: 1, x: 0, y: 0, rotate: 0 }"
            :delay="index * 100 + 400"
            :visible="{
              opacity: 1,
              x: -(-30 + index * 8),
              y: -(8 + index * 3),
              rotate: -(5 + index * 8),
            }"
            style="width: 30vmin; height: 45vmin" />
        </div>
      </div>

      <!-- Right side - Content -->
      <div class="col-xs-12 col-md-6 flex column justify-center items-center q-pa-md">
        <div
          class="content-section"
          v-motion
          :initial="{ opacity: 0, y: -30 }"
          :enter="{ opacity: 1, y: 0 }"
          :duration="800"
          :delay="200">
          <h1 class="text-h2 text-primary q-mb-md hero-title">Eterna Invites</h1>
          <p class="text-h6 text-grey-8 q-mb-lg hero-subtitle">
            {{ $t('main.heroSubtitle') }}
          </p>

          <!-- Search Input -->
          <div class="search-section q-mb-lg">
            <q-input
              v-model="searchText"
              outlined
              :placeholder="$t('main.searchPlaceholder')"
              class="search-input"
              dense
              autocomplete="off"
              autocapitalize="off"
              list="autocompleteOff"
              autocorrect="off"
              spellcheck="false">
              <template v-slot:prepend>
                <q-icon name="search" />
              </template>
              <template v-slot:append>
                <q-btn
                  flat
                  round
                  dense
                  icon="arrow_forward"
                  color="primary"
                  @click="handleSearch" />
              </template>
            </q-input>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { ref } from 'vue'
  import { useI18n } from 'vue-i18n'

  // Composables
  const { t } = useI18n()

  // Reactive data
  const searchText = ref('')

  // Card images data
  const cardImages = ref([
    {
      src: 'src/assets/images/card1.jpg',
      alt: 'Card 1',
      title: 'Invitación 1',
    },
    {
      src: 'src/assets/images/card1.jpg',
      alt: 'Card 2',
      title: 'Invitación 2',
    },
    {
      src: 'src/assets/images/card1.jpg',
      alt: 'Card 3',
      title: 'Invitación 3',
    },
    {
      src: 'src/assets/images/card1.jpg',
      alt: 'Card 4',
      title: 'Invitación 4',
    },
    {
      src: 'src/assets/images/card1.jpg',
      alt: 'Card 5',
      title: 'Invitación 5',
    },
  ])

  // Methods
  const handleSearch = () => {
    if (searchText.value.trim()) {
      // Handle search logic here
      console.log('Searching for:', searchText.value)
    }
  }
</script>

<style scoped lang="scss" src="./Container1.scss"></style>
