export default {
  questionnaire: {
    common: {
      test: 'Salut le monde',
      404: "n'a pas été trouvé",
      403: 'Demande interdite',
      500: "La communication avec le serveur ne fonctionne pas. Avez-vous accès à l'internet ?"
    },
    thanks: {
      title: "Nous vous remercions vivement d'avoir participé à notre questionnaire.",
      subtitle:
        "Votre contribution nous permet d'améliorer la qualité du traitement. Si vous avez des questions, n'hésitez pas à nous contacter.",
      contactInsel: 'Contactez-nous',
      newQuestionnaire: 'Remplir un nouveau questionnaire'
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
      questionOptionalExplanation: 'Indique que cette question est facultative',
      clear: 'éteindre',
      submitError: "Une erreur s'est produite. Veuillez réessayer"
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
