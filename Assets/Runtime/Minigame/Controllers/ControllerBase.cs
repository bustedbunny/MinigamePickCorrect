namespace MinigamePickCorrect
{
    public abstract class ControllerBase
    {
        public ControllerBase(MinigamePickCorrect parent)
        {
            this.parent = parent;
        }
        protected MinigamePickCorrect parent;
        public virtual void Init() { }
        public virtual void NotifyLevelIsFinished() { }
        public virtual void NotifyGameIsOver() { }
    }
}
