export default {
  questionnaire: {
    common: {
      test: 'Hallo Welt!',
      404: 'konnte nicht gefunden werden',
      500: 'Kommunikation mit dem Server ist fehlgeschlagen. Haben sie kein Internet?'
    },
    thanks: {
      title: 'Wir danken ihnen vielmals für die Teilnahme an der Umfrage',
      subtitle:
        'Mit ihrem Beitrag können wir die Qualität der Behandlung stetig verbessern. Falls sie noch Fragen haben können sie uns diese gerne kontaktieren',
      contactInsel: 'Kontakt',
      newQuestionnaire: 'Neue Umfrage ausfüllen'
    },
    navigation: {
      language: 'Sprache',
      startQuestionnaire: 'Umfrage starten',
      questionnaire: 'Umfrage',
      invitationCode: 'Einladungscode',
      provideCode: 'Bitte geben sie ihren Einladungscode ein',
      submitQuestionnaire: 'Umfrage abschicken',
      clearAnswers: 'Antworten löschen',
      abortQuestionnaire: 'Umfrage abbrechen',
      codeAlreadyUsed: 'Anfrage verboten, dieser Einladungscode wurde schon verwendet',
      questionOptionalExplanation: 'Bedeutet diese Frage ist optional',
      clear: 'löschen',
      submitError: 'Ein Fehler ist aufgetreten. Bitte versuche es erneut'
    },
    validation: {
      fieldRequired: 'Dieses Feld ist erforderlich',
      fieldIsNumber: 'Dieses Feld muss eine positive Nummer sein',
      eightDigits: 'Der Einladungscode muss 8 Zeichen lang sein'
    }
  },
  admin: {
    loginComponent: {
      authenticate: 'Authentifizieren',
      login: 'Anmelden',
      alerts: {
        close: 'Schliessen',
        logoutFail: 'Etwas ist schiefgelaufen, bitte versuchen sie es erneut',
        authenticated: 'Erfolgreich authentifiziert',
        userRequired: 'Sie müssen authentifiziert sein',
        adminRequired: 'Sie müssen Admin sein'
      }
    }
  }
}
