const languages = ['de', 'en', 'fr']
const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789'
const questions: any[] = [
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
  },
  {
    answers: [],
    text: 'Ich hatte das Gefühl, dass das Pflegeteam meine spezifischen Bedürfnisse richtig verstanden hat.',
    subtext: '',
    optional: false,
    questiontype: 'Likert',
    options: []
  },
  {
    answers: [],
    text: 'Ich hatte die Möglichkeit, dem Behandlungsteam wichtige Aspekte meines Gesundheitsproblems mitzuteilen.',
    subtext: '',
    optional: false,
    questiontype: 'Likert',
    options: []
  },
  {
    answers: [],
    text: 'Ich hatte das Gefühl, dass meine gesundheitlichen Bedenken vom Pflegeteam ernst genommen wurden.',
    subtext: '',
    optional: false,
    questiontype: 'Likert',
    options: []
  },
  {
    answers: [],
    text: 'Das Behandlungsteam erklärte mir, wie sie mein Gesundheitsproblem erklärten.',
    subtext: '',
    optional: false,
    questiontype: 'Likert',
    options: []
  },
  {
    answers: [],
    text: 'I felt that the explanation of my health problem that was given to me was correct.',
    subtext: '',
    optional: false,
    questiontype: 'Likert',
    options: []
  },
  {
    answers: [],
    text: 'Ich hatte Vertrauen in die diagnostischen Fähigkeiten des Pflegeteams, das an der Erklärung meines Gesundheitsproblems arbeitete.',
    subtext: '',
    optional: false,
    questiontype: 'Likert',
    options: []
  },
  {
    answers: [],
    text: 'Ich hatte den Eindruck, dass das Pflegeteam mir mein Gesundheitsproblem genau erklärt hat.',
    subtext: '',
    optional: false,
    questiontype: 'Likert',
    options: []
  },
  {
    answers: [],
    text: 'Ich hatte das Gefühl, dass mir die Erklärung meines Gesundheitsproblems gut vermittelt wurde.',
    subtext: '',
    optional: false,
    questiontype: 'Likert',
    options: []
  },
  {
    answers: [],
    text: 'Die Erklärung beschrieb, was ich von meinem Gesundheitsproblem zu erwarten hatte, unabhängig davon, ob es sich verschlimmerte, gleich blieb oder sich auflöste.',
    subtext: '',
    optional: false,
    questiontype: 'Likert',
    options: []
  },
  {
    answers: [],
    text: 'Aufgrund dessen, was mir über mein Gesundheitsproblem mitgeteilt wurde, hatte ich das Gefühl, dass ich einem Freund mein Gesundheitsproblem erklären könnte.',
    subtext: '',
    optional: false,
    questiontype: 'Likert',
    options: []
  },
  {
    answers: [],
    text: 'Die Informationen, die ich erhielt, waren für mich leicht zu verstehen.',
    subtext: '',
    optional: false,
    questiontype: 'Likert',
    options: []
  },
  {
    answers: [],
    text: 'Das Pflegeteam prüfte, wie ich die Erklärung meines Gesundheitsproblems interpretierte.',
    subtext: '',
    optional: false,
    questiontype: 'Likert',
    options: []
  },
  {
    answers: [],
    text: 'Mir wurde mitgeteilt, was ich wissen muss, um nach dem Besuch die nächsten Schritte zu unternehmen.',
    subtext: '',
    optional: false,
    questiontype: 'Likert',
    options: []
  },
  {
    answers: [],
    text: 'Durch die Kommunikation, die ich erhielt, hatte ich das Gefühl, einen Plan zu haben, den ich befolgen konnte, zum Beispiel wusste ich, welche Ärzte ich aufsuchen musste.',
    subtext: '',
    optional: false,
    questiontype: 'Likert',
    options: []
  }
]

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
    descriptionForCustomer: `Dieser Fragebogen [${identifier}] dient zum Testen, er wurde gefüllt mit zufälligen Fragen`,
    validAfterDays: 15,
    validForDays: 10
  }
  console.log(questionnaire)
  return questionnaire
}
