import { customRef } from 'vue'

/**
 * Debounced ref that only updates its value after a given amount of milliseconds.
 * Default is 800 milliseconds
 * @param value Value to set the reactive variable to
 * @param delay Delay in milliseconds until the ref updates
 * @returns
 */
export function useDebouncedRef(value: any, delay = 800) {
  let timeout: string | number | NodeJS.Timeout | undefined
  return customRef((track, trigger) => {
    return {
      get() {
        track()
        return value
      },
      set(newValue) {
        clearTimeout(timeout)
        timeout = setTimeout(() => {
          value = newValue
          trigger()
        }, delay)
      }
    }
  })
}
