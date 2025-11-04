<template>
  <div class="audio-player">
    <button class="play-pause" @click="togglePlay" :aria-label="isPlaying ? 'Pause' : 'Play'">
      <svg
        v-if="!isPlaying"
        width="18px"
        height="20px"
        viewBox="0 0 18 20"
        xmlns="http://www.w3.org/2000/svg">
        <g fill="currentcolor">
          <path
            d="M17.29,9.02 C18.25,9.56 18.25,10.44 17.29,10.98 L1.74,19.78 C0.78,20.33 0,19.87 0,18.76 L0,1.24 C0,0.13 0.78,-0.32 1.74,0.22 L17.29,9.02 Z"></path>
        </g>
      </svg>
      <svg v-else width="18px" height="20px" viewBox="0 0 18 20" xmlns="http://www.w3.org/2000/svg">
        <g fill="currentcolor">
          <rect x="0" y="0" width="6" height="20"></rect>
          <rect x="12" y="0" width="6" height="20"></rect>
        </g>
      </svg>
    </button>
    <div class="progress-bar">
      <input
        type="range"
        min="0"
        :max="audioDuration"
        step="0.01"
        v-model="currentTime"
        @input="seekAudio"
        :aria-valuemin="0"
        :aria-valuemax="audioDuration"
        :aria-valuenow="currentTime" />
    </div>
  </div>
</template>

<script setup lang="ts">
  import { ref, onMounted, onUnmounted } from 'vue'

  const audio = new Audio('https://furrowthebrow.github.io/demo/TakesMeBack.mp3')
  const isPlaying = ref(false)
  const currentTime = ref(0)
  const audioDuration = ref(0)

  const togglePlay = () => {
    if (isPlaying.value) {
      audio.pause()
    } else {
      audio.play()
    }
    isPlaying.value = !isPlaying.value
  }

  const updateProgress = () => {
    currentTime.value = audio.currentTime
  }

  const seekAudio = () => {
    audio.currentTime = currentTime.value
  }

  onMounted(() => {
    audio.addEventListener('timeupdate', updateProgress)
    audio.addEventListener('loadedmetadata', () => {
      audioDuration.value = audio.duration
    })
  })

  onUnmounted(() => {
    audio.removeEventListener('timeupdate', updateProgress)
  })
</script>

<style scoped>
  .audio-player {
    display: flex;
    align-items: center;
    gap: 10px;
  }

  .play-pause {
    background: none;
    border: none;
    cursor: pointer;
    color: #333;
    display: flex;
    align-items: center;
    justify-content: center;
    width: 40px;
    height: 40px;
  }

  .progress-bar input[type='range'] {
    width: 100%;
    appearance: none;
    background: #ddd;
    height: 5px;
    border-radius: 5px;
    outline: none;
    cursor: pointer;
  }

  .progress-bar input[type='range']::-webkit-slider-thumb {
    appearance: none;
    width: 10px;
    height: 10px;
    background: #333;
    border-radius: 50%;
    cursor: pointer;
  }
</style>
