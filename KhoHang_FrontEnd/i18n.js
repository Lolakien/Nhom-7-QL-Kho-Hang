import i18next from 'i18next'
import { initReactI18next } from 'react-i18next'

import translationEN from '@/locales/en/translation.json'
import translationVN from '@/locales/vn/translation.json'

const resources = {
  en: {
    translation: translationEN
  },
  vn: {
    translation: translationVN
  }
}

i18next.use(initReactI18next).init({
  resources,
  lng: 'vn',
  fallbackLng: 'vn'
})

export default i18next
