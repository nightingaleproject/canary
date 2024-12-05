import { toast } from 'react-semantic-toasts';

export function connectionErrorToast(error) {
    let errMsg = error?.response?.data?.errorDetails
    toast({
        type: 'error',
        icon: 'exclamation circle',
        title: 'Error!',
        description: 'Error: "' + errMsg + '"',
        time: 10000,
    });
}
