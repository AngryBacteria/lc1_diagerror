import { createI18n } from 'vue-i18n'
import messages from '../locales/index'

/**
 * Defines the structure of the language definition JSONs
 */
export const APP_LANGUAGES: MessageLanguages[] = ['de', 'fr', 'en']
export type MessageLanguages = keyof typeof messages
export type MessageSchema = (typeof messages)['de']

declare module 'vue-i18n' {
  export interface DefineLocaleMessage extends MessageSchema {}
  export interface DefineDateTimeFormat {}
  export interface DefineNumberFormat {}
}

/**
 * Creates the i18n instance and exports it
 */
export const i18n = createI18n<[MessageSchema], MessageLanguages>({
  locale: APP_LANGUAGES[0],
  legacy: false,
  messages: messages
})
