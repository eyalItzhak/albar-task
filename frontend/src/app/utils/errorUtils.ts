

export const ErrorStandartize = (error :any): string => {
    const errors = error.error.errors
      return Object.entries(errors).map(([key, messages]) => `${key} - ${messages}`).join('\n');
}