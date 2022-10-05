using System;
using System.Runtime.InteropServices;

namespace UsefulCsharpCommonsUtils.cli
{
    public class NorefMessageBox
    {

        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        private static extern int MessageBox(IntPtr h, string m, string c, int type);

        public enum MessageBoxButton
        {
            //
            // Résumé :
            //     La boîte de message affiche un OK bouton.
            OK = 0,
            //
            // Résumé :
            //     Affiche la boîte de message OK et Annuler boutons.
            OKCancel = 1,
            //
            // Résumé :
            //     Affiche la boîte de message Abandonner, Recommencer, et Ignorer boutons.
            AbortRetryIgnore = 2,
            //
            // Résumé :
            //     Affiche la boîte de message Oui, non, et Annuler boutons.
            YesNoCancel = 3,
            //
            // Résumé :
            //     Affiche la boîte de message Oui et non boutons.
            YesNo = 4,
            //
            // Résumé :
            //     Affiche la boîte de message Recommencer et Annuler boutons.
            RetryCancel = 5,
            //
            // Résumé :
            //     Affiche la boîte de message Annuler, Recommencer et Continuer boutons.
            CancelRetryContinu = 6
        }

        public enum MessageBoxResult
        {
            //
            // Résumé :
            //     La boîte de message ne retourne aucun résultat.
            None = 0,
            //
            // Résumé :
            //     La valeur de résultat de la boîte de message est OK.
            OK = 1,
            //
            // Résumé :
            //     La valeur de résultat de la boîte de message est Annuler.
            Cancel = 2,
            //
            // Résumé :
            //     La valeur de résultat de la boîte de message est Abandonner.
            Abort = 3,
            //
            // Résumé :
            //     La valeur de résultat de la boîte de message est Recommencer.
            Retry = 4,
            //
            // Résumé :
            //     La valeur de résultat de la boîte de message est Ignorer.
            Ignore = 5,
            //
            // Résumé :
            //     La valeur de résultat de la boîte de message est Oui.
            Yes = 6,
            //
            // Résumé :
            //     La valeur de résultat de la boîte de message est non.
            No = 7,
            //
            // Résumé :
            //     La valeur de résultat de la boîte de message est Recommencer, dans le cas ou le type de message est CancelRetryContinu.
            RetryInCancelRetryContinu = 10,
            //
            // Résumé :
            //     La valeur de résultat de la boîte de message est continue.
            Continue = 11
        }

        public static MessageBoxResult Show(string message, string title = "Titre",
            MessageBoxButton button = MessageBoxButton.OKCancel)
        {
            return (MessageBoxResult) MessageBox(IntPtr.Zero, message, title, (int)button);
        }
   

    }
}
