using System.Collections.Generic;

namespace Game.Scripts.Tutorial
{
    public class TutorialController
    {
        //Реализовать тутириал контроллер который содержит шаги туториала и дает функциионал для их выполнения
        //С переходом на следующий этап (переходы через eventBus)
        //Должен прокидываться через zenject давать выполнять шаги сторонним объектам: рычагам, кнопкам, мини играм
        
        private IDictionary<int, TutorialStep> tutorialSteps;
    }
}