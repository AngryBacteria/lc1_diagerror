import type { MessageSchema, MessageLanguages } from '../plugins/i18n'
import { useI18n } from 'vue-i18n'

/**
 * Creates a typed version of the i18n object for better code safety
 * @returns Typed version of the i18n object 
 */
export function useTypedI18n() {
  return useI18n<{ message: MessageSchema }, MessageLanguages>()
}
