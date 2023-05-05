export default {
  questionnaire: {
    common: {
      test: 'Hello World',
      404: 'could not be found',
      403: 'Request forbidden',
      500: 'Communication with the server failed. Do you have internet?'
    },
    thanks: {
      title: 'We thank you very much for participating in the survey',
      subtitle:
        'With your contribution we can constantly improve the quality of treatment. If you have any questions you are welcome to contact us',
      contactInsel: 'Contact us',
      newQuestionnaire: 'Fill out a new survey'
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
      questionOptionalExplanation: 'Means this question is optional',
      clear: 'clear',
      submitError: 'An error has occurred. Please try again'
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
  },
  common: {
    '404': "You didnt break the internet, but we can't find what you are looking for",
    back: 'Go back to the homepage'
  }
}
