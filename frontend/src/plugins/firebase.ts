import { initializeApp } from 'firebase/app'

const firebaseConfig = {
  apiKey: 'AIzaSyAqbEVHkqsqEbWlkT95m2OjgNuH77R3c4w',
  authDomain: 'lc1-diagerror.firebaseapp.com',
  projectId: 'lc1-diagerror',
  storageBucket: 'lc1-diagerror.appspot.com',
  messagingSenderId: '604241069161',
  appId: '1:604241069161:web:9d0a690a5496d181414ef8'
}

export const firebaseApp = initializeApp(firebaseConfig)
