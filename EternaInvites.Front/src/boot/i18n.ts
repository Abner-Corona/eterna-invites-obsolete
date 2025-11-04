import { defineBoot } from '#q-app/wrappers'
import { createI18n } from 'vue-i18n'

import en from '../i18n/en.json'
import es from '../i18n/es.json'
const messages = {
  en,
  es,
}
export type MessageLanguages = keyof typeof messages
// Type-define 'en-US' as the master schema for the resource
export type MessageSchema = (typeof messages)['en']

// See https://vue-i18n.intlify.dev/guide/advanced/typescript.html#global-resource-schema-type-definition
/* eslint-disable @typescript-eslint/no-empty-object-type */
declare module 'vue-i18n' {
  // define the locale messages schema
  export interface DefineLocaleMessage extends MessageSchema {}

  // define the datetime format schema
  export interface DefineDateTimeFormat {}

  // define the number format schema
  export interface DefineNumberFormat {}
}
/* eslint-enable @typescript-eslint/no-empty-object-type */
export const i18n = createI18n<{ message: MessageSchema }, MessageLanguages>({
  locale: 'es',

  legacy: false,

  messages,
})
export default defineBoot(({ app }) => {
  // Set i18n instance on app
  app.use(i18n)
  app.config.globalProperties.$i18n = i18n.global
})
