const languages = ['de', 'en', 'fr']
const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789'
const questions = [
  {
    answers: [],
    text: 'Wann waren sie das letzte mal im Spital?',
    subtext: 'Man kann hier einen beliebigen String eingeben',
    optional: false,
    questiontype: 'FreeText',
    options: []
  },
  {
    answers: [],
    text: 'Wie viel Schmerzen spüren sie von 1-10',
    subtext: 'Man kann hier einen beliebigen String eingeben',
    optional: false,
    questiontype: 'FreeText',
    options: []
  },
  {
    answers: [],
    text: 'Wo waren sie das letzte mal im Spital?',
    subtext: 'Man kann hier einen beliebigen String eingeben',
    optional: false,
    questiontype: 'FreeText',
    options: []
  },
  {
    answers: [],
    text: 'Wieso waren sie das letzte mal im Spital?',
    subtext: 'Man kann hier einen beliebigen String eingeben',
    optional: false,
    questiontype: 'FreeText',
    options: []
  },
  {
    answers: [],
    text: 'Wie hat ihnen der Aufenthalt gefallen?',
    subtext: 'Man kann hier einen beliebigen String eingeben',
    optional: false,
    questiontype: 'FreeText',
    options: []
  },
  {
    answers: [],
    text: 'Wurden sie gut betreut?',
    subtext: 'Man kann hier einen beliebigen String eingeben',
    optional: false,
    questiontype: 'FreeText',
    options: []
  },
  {
    answers: [],
    text: 'Wie lange mussten sie warten?',
    subtext: 'Man kann hier einen beliebigen String eingeben',
    optional: false,
    questiontype: 'FreeText',
    options: []
  },
  {
    answers: [],
    text: 'Das ist eine Freitextfrage',
    subtext: 'Man kann hier einen beliebigen String eingeben',
    optional: false,
    questiontype: 'FreeText',
    options: []
  },
  {
    answers: [],
    text: 'Das ist eine Likert-Skala Frage',
    subtext: 'Man kann hier die 5 Likert-Punkte angeben',
    optional: false,
    questiontype: 'Likert',
    options: []
  },
  {
    answers: [],
    text: 'Das ist eine Single-Choice Frage',
    subtext: 'Die möglichen Antworten werden im Fragebogen definiert',
    optional: false,
    questiontype: 'SingleChoice',
    index: 2,
    options: [
      {
        index: 0,
        value: 'Erste Antwortmöglichkeit'
      },
      {
        index: 1,
        value: 'Zweite Antwortmöglichkeit'
      },
      {
        index: 2,
        value: 'Dritte Antwortmöglichkeit 2'
      },
      {
        index: 3,
        value: 'Vierte Antwortmöglichkeit 3'
      }
    ]
  }
]

const huh = {
  identifier: 'TF',
  language: 'DE',
  title: 'Testfragebogen für die Entwicklung',
  description: 'Testfragebogen mit jeweils einer Frage für jeden definierten Fragetypen',
  descriptionForCustomer:
    'Dieser Fragebogen dient zum Testen der Funktionen dieser App. Alle verfügbaren Fragetypen sind vorhanden',
  validAfterDays: 15,
  validForDays: 10
}

export function useRandomQuestionnaire(amount: number) {
  const outputQuestions = []
  for (let index = 0; index < amount; index++) {
    const item = questions[Math.floor(Math.random() * questions.length)]
    item.index = index
    outputQuestions.push(item)
  }

  const language = languages[Math.floor(Math.random() * languages.length)]
  const identifier =
    characters[Math.floor(Math.random() * characters.length)] +
    characters[Math.floor(Math.random() * characters.length)]

  const questionnaire = {
    questions: outputQuestions,
    identifier: identifier,
    language,
    title: `Testfragebogen [${identifier}] für die Entwicklung`,
    description: `Testfragebogen [${identifier}] gefüllt mit zufälligen Fragen`,
    descriptionForCustomer:
      `Dieser Fragebogen [${identifier}] dient zum Testen, er wurde gefüllt mit zufälligen Fragen`,
    validAfterDays: 15,
    validForDays: 10
  }
  console.log(questionnaire)
  return questionnaire
}
