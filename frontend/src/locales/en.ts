export default {
  questionnaire: {
    common: {
      test: 'Hello World',
      404: 'could not be found',
      403: 'Request forbidden',
      500: 'Communication with the server failed. Do you have internet?'
    },
    navigation: {
      language: 'Language',
      startQuestionnaire: 'Start questionnaire',
      questionnaire: 'Questionnaire',
      invitationCode: 'Invitation code',
      provideCode: 'Please provide your invitation code',
      submitQuestionnaire: 'Submit Questionnaire',
      clearAnswers: 'Clear answers',
      abortQuestionnaire: 'Abort Questionnaire',
      codeAlreadyUsed: 'Request forbidden, this invitation code has already been used',
    },
    validation: {
      fieldRequired: 'This field is required',
      fieldIsNumber: 'This field needs to be a positive number',
      eightDigits: 'The invitation code needs to have 8 digits'
    }
  },
  admin: {
    loginComponent: {
      authenticate: 'Authenticate',
      login: 'Login',
      alerts: {
        close: 'Close',
        logoutFail: 'Something went wrong, please try again',
        authenticated: 'Successfully authenticated',
        userRequired: 'You have to be authenticated',
        adminRequired: 'You have to  be an admin'
      }
    }
  }
}
