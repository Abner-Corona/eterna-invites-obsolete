<template>
  <section class="demos-section q-py-xl">
    <div class="q-container">
      <h2 class="text-h3 text-center q-mb-lg animate-from-bottom">
        {{ $t('main.demosTitle') }}
      </h2>
      <p
        class="text-subtitle1 text-center q-mb-xl animate-from-bottom"
        style="max-width: 700px; margin: 0 auto">
        {{ $t('main.demosSubtitle') }}
      </p>

      <div class="row q-col-gutter-lg justify-center">
        <div v-for="(demo, index) in demos" :key="index" class="col-12 col-md-4 col-lg-3 q-mb-md">
          <q-card
            class="demo-card animate-scale"
            :style="`transition-delay: ${index * 0.1}s`"
            @click="openDemo(demo)">
            <q-img :src="demo.thumbnail" :ratio="4 / 3" basic>
              <div class="absolute-bottom text-subtitle2 text-center bg-black-opacity">
                {{ $t(demo.nameKey) }}
              </div>
            </q-img>

            <q-card-section>
              <div class="text-h6">{{ $t(demo.nameKey) }}</div>
              <div class="text-subtitle2">{{ $t(demo.categoryKey) }}</div>
            </q-card-section>

            <q-card-section class="q-pt-none">
              {{ $t(demo.descriptionKey) }}
            </q-card-section>

            <q-card-actions align="right">
              <q-btn
                flat
                color="primary"
                :label="$t('main.viewDemo')"
                @click.stop="openDemo(demo)" />
            </q-card-actions>

            <q-badge color="primary" floating>
              {{ $t(demo.isNew ? 'main.new' : 'main.popular') }}
            </q-badge>
          </q-card>
        </div>
      </div>

      <div class="text-center q-mt-xl">
        <q-btn
          color="primary"
          :label="$t('main.viewAllDemos')"
          size="lg"
          to="/gallery"
          class="animate-from-bottom" />
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
  const router = useRouter()
  // Demos data
  const demos = ref([
    {
      thumbnail: 'https://via.placeholder.com/300x200',
      nameKey: 'main.demo1_name',
      categoryKey: 'main.demo1_category',
      descriptionKey: 'main.demo1_description',
      isNew: true,
    },
    {
      thumbnail: 'https://via.placeholder.com/300x200',
      nameKey: 'main.demo2_name',
      categoryKey: 'main.demo2_category',
      descriptionKey: 'main.demo2_description',
      isNew: false,
    },
    {
      thumbnail: 'https://via.placeholder.com/300x200',
      nameKey: 'main.demo3_name',
      categoryKey: 'main.demo3_category',
      descriptionKey: 'main.demo3_description',
      isNew: false,
    },
  ])

  // Function to open demo
  const openDemo = (demo: { nameKey: string }) => {
    router.push(`/demo/${demo.nameKey}`)
  }
</script>

<style scoped>
  /* Demos Section */
  .demos-section {
    background: #f5f5f5;
  }

  .demo-card {
    cursor: pointer;
    transition: transform 0.3s;
  }

  .demo-card:hover {
    transform: translateY(-10px);
  }

  .bg-black-opacity {
    background: rgba(0, 0, 0, 0.6);
    padding: 8px;
  }
</style>
