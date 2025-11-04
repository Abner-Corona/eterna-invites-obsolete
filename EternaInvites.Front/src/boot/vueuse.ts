import { defineBoot } from '#q-app/wrappers'
import { MotionPlugin } from '@vueuse/motion'
// "async" is optional;
// more info on params: https://v2.quasar.dev/quasar-cli-vite/boot-files
export default defineBoot(async ({ app }) => {
  // something to do
  app.use(MotionPlugin, {})
})
