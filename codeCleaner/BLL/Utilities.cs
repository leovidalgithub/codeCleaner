using System;
using System.Windows.Forms;

namespace codeCleaner.BLL {
    public static class Utilities {
        public static void CrossFade(Form FormToOut, Form FormtToIn) {
            if (FormToOut != null) FormToOut.Refresh();
            if (FormtToIn != null) FormtToIn.Refresh();
            do {
                if (FormToOut != null && FormToOut.Opacity > .01) FormToOut.Opacity -= 0.05;
                if (FormtToIn != null && FormtToIn.Opacity < 1) FormtToIn.Opacity += 0.05;
                System.Threading.Thread.Sleep(40);
            } while ((FormToOut != null && FormToOut.Opacity > .01) || (FormtToIn != null && FormtToIn.Opacity < 1));
            if (FormToOut != null) FormToOut.Refresh();
            if (FormtToIn != null) FormtToIn.Refresh();
        }
    }
}
