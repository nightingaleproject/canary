import { toast } from 'react-semantic-toasts';

export function connectionErrorToast(error) {
  toast({
    type: 'error',
    icon: 'exclamation circle',
    title: 'Error!',
    description: 'There was an error communicating with Canary. The error was: "' + error + '"',
    time: 5000,
  });
}
