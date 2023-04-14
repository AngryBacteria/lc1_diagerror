export default {
  questionnaire: {
    common: {
      test: 'Hallo Welt!'
    },
    navigation: {
      language: 'Sprache',
      startQuestionnaire: 'Umfrage starten',
      questionnaire: 'Umfrage',
      invitationCode: 'Einladungscode',
      provideCode: 'Bitte geben sie ihren Einladungscode ein',
      submitQuestionnaire: 'Umfrage abschicken',
      clearAnswers: 'Antworten löschen',
      abortQuestionnaire: 'Umfrage abbrechen'
    },
    validation: {
      fieldRequired: 'Dieses Feld ist erforderlich',
      fieldIsNumber: 'Dieses Feld muss eine positive Nummer sein',
      //TODO fix after sprint 2/3
      sixDigits: 'Der Einladungscode muss 6 Zeichen lang sein'
    }
  },
  admin: {
    loginComponent: {
      authenticate: 'authentifizieren',
      login: 'anmelden',
      alerts: {
        close: 'Schliessen',
        logoutFail: 'Etwas ist schiefgelaufen beim ausloggen',
        authenticated: 'Erfolgreich authentifiziert',
        userRequired: 'Sie müssen authentifiziert sein',
        adminRequired: 'Sie müssen Admin sein'
      }
    }
  }
}
