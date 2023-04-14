export default {
  questionnaire: {
    common: {
      test: 'Hello World'
    },
    navigation: {
      language: 'Language',
      startQuestionnaire: 'Start questionnaire',
      questionnaire: 'Questionnaire',
      invitationCode: 'Invitation code',
      provideCode: 'Please provide your invitation code',
      submitQuestionnaire: 'Submit Questionnaire',
      clearAnswers: 'Clear answers',
      abortQuestionnaire: 'Abort Questionnaire'
    },
    validation: {
      fieldRequired: 'This field is required',
      fieldIsNumber: 'This field needs to be a positive number',
      sixDigits: 'The invitation code needs to have 6 digits'
    }
  },
  admin: {
    loginComponent: {
      authenticate: 'authenticate',
      login: 'login',
      alerts: {
        close: 'Close',
        logoutFail: 'Something went wrong trying to logout',
        authenticated: 'Successfully authenticated',
        userRequired: 'You have to be authenticated',
        adminRequired: 'You have to  be an admin'
      }
    }
  }
}
