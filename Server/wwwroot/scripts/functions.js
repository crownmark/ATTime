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

window.appointmentAudio = {
    audio: null,
    initialized: false,

    initialize: function () {
        if (!this.audio) {
            this.audio = new Audio('/sounds/appointment-reminder-soft-chime.mp3');
            this.audio.preload = 'auto';
        }

        this.initialized = true;
    },

    play: function () {
        if (!this.audio) {
            this.initialize();
        }

        this.audio.currentTime = 0;

        return this.audio.play().catch(error => {
            console.warn("Unable to play appointment reminder sound:", error);
        });
    },

    stop: function () {
        if (this.audio) {
            this.audio.pause();
            this.audio.currentTime = 0;
        }
    }
};