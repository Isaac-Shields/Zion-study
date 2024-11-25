

public class problemObj
{
    private string problem;
    private string answer;
    private int problemId;

    public problemObj()
    {

    }

    public string getProblem()
    {
        return problem;
    }

    public string getAnswer()
    {
        return answer;
    }

    public void setProblem(string p)
    {
        problem = p;
    }

    public void setAnswer(string a)
    {
        answer = a;
    }

    public void setId(int id)
    {
        problemId = id;
    }

    public int getId()
    {
        return problemId;
    }
}