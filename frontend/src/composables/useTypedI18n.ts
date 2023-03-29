import type { MessageSchema, MessageLanguages } from '../plugins/i18n'
import { useI18n } from 'vue-i18n'

export function useTypedI18n() {
  return useI18n<{ message: MessageSchema }, MessageLanguages>()
}
