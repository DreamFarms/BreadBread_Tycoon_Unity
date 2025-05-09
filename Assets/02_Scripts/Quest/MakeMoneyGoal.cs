internal class MakeMoneyGoal : IQuestGoal
{
    private int targetMoney;
    private int currentMoeny;

    public MakeMoneyGoal(int initialTarget)
    {
        this.targetMoney = initialTarget;
    }
    public void UpdateProgress(params object[] args)
    {

    }

    public bool IsCompleted()
    {
        throw new System.NotImplementedException();
    }

    public void ResetGoal()
    {
        throw new System.NotImplementedException();
    }

}