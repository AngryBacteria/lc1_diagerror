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

export interface Question {
  questionId?: number
  questionnaireId: number
  answers: Answer[]
  text: string
  subtext: string
  optional: boolean
  questiontype: 'Likert' | 'FreeText' | 'SingleChoice'
  options: Option[]
}

export interface Answer {
  answerId?: number
  questionId: number
  text: string
  date: string
  invitationId: string
}

export interface Option {
  optionId?: number
  index: number
  value: string
}
