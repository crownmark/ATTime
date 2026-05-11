class Helpers {
    static dotNetHelper;

    static setDotNetHelper(value) {
        Helpers.dotNetHelper = value;

    }
    static async playNotification() {
        var audio = new Audio('/sounds/ChatAlert1.wav');
        audio.play();
    }
    static async playUnacknowledgedNotification() {
        var audio = new Audio('/sounds/ChatAlert2.wav');
        audio.play();
    }

    static async closeTimeEntryWindowPanel(dialogId) {
        await Helpers.dotNetHelper.invokeMethodAsync('CloseDialogFromJS', dialogId);
    }

    

    
}

window.Helpers = Helpers;