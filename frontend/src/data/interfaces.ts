/**
 * Paginated response of the backend for Questionnaires
 */
export interface PaginatedQuestionnaire {
  pageCount: number
  data: Questionnaire[]
}

/**
 * Questionnaire as defined in the backend database
 */
export interface Questionnaire {
  questionnaireId?: number
  questions: Question[]
  identifier: string
  language: string
  title: string
  description: string
  descriptionForCustomer: string
  validAfterDays: number
  validForDays: number
}

/**
 * Question as defined in the backend database
 */
export interface Question {
  questionId?: number
  questionnaireId: number
  answers: Answer[]
  text: string
  subtext: string
  optional: boolean
  questiontype: 'Likert' | 'FreeText' | 'SingleChoice' | 'MultipleChoice'
  options: Option[]
  index: number
}

/**
 * Answer as defined in the backend database
 */
export interface Answer {
  answerId?: number
  questionId: number
  text: string
  date: string
  invitationId: string
}

/**
 * Answer-Option as defined in the backend database
 */
export interface Option {
  optionId?: number
  index: number
  value: string
}
