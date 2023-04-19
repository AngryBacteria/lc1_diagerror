export default {
  questionnaire: {
    common: {
      test: 'Salut le monde',
      404: "n'a pas été trouvé",
      403: 'Demande interdite',
      500: "La communication avec le serveur ne fonctionne pas. Avez-vous accès à l'internet ?"
    },
    navigation: {
      language: 'Langue',
      startQuestionnaire: 'Commencer le questionnaire',
      questionnaire: 'Questionnaire',
      invitationCode: "Code d'invitation",
      provideCode: "Veuillez entrer votre code d'invitation",
      submitQuestionnaire: 'Envoyer le questionnaire',
      clearAnswers: 'Effacer les questions',
      abortQuestionnaire: 'Abandonner le questionnaire',
      codeAlreadyUsed: "Demande interdite, ce code d'invitation a déjà été utilisé",
    },
    validation: {
      fieldRequired: 'Ce formulaire est obligatoire',
      fieldIsNumber: 'Ce formulaire doit être un nombre positif',
      eightDigits: "Le code d'invitation doit contenir 8 caractères"
    }
  },
  admin: {
    loginComponent: {
      authenticate: 'Authentifier',
      login: 'login',
      alerts: {
        close: 'Fermer',
        logoutFail: "Un problème s'est produit, veuillez réessayer",
        authenticated: 'Authentifié avec succès',
        userRequired: 'Vous devez être authentifié',
        adminRequired: 'Vous devez être administrateur'
      }
    }
  }
}
