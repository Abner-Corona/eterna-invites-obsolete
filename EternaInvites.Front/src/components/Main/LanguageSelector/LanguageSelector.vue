<template>
  <q-btn-dropdown flat icon="language" class="q-ml-md">
    <q-list>
      <q-item clickable v-close-popup @click="changeLanguage('es')">
        <q-item-section>
          <q-item-label>{{ $t('header.español') }}</q-item-label>
        </q-item-section>
      </q-item>
      <q-item clickable v-close-popup @click="changeLanguage('en')">
        <q-item-section>
          <q-item-label>{{ $t('header.english') }}</q-item-label>
        </q-item-section>
      </q-item>
    </q-list>
  </q-btn-dropdown>
</template>

<script setup lang="ts">
  import { useI18n } from 'vue-i18n'
  import { useLanguageStore } from 'src/stores/languageStore'

  // Composables
  const { locale } = useI18n()
  const languageStore = useLanguageStore()

  // Methods
  const changeLanguage = (lang: string) => {
    languageStore.changeLanguage(lang)
    locale.value = lang
  }
</script>

<style scoped lang="scss" src="./LanguageSelector.scss"></style>
