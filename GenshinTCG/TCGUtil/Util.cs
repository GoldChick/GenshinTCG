namespace TCGUtil
{
    /// <summary>
    /// ������ֻ��Ҫ�ڿ���̨�д�ӡ״̬�Ķ���
    /// </summary>
    public interface IPrintable
    {
        /// <summary>
        /// ��ӡ�����class��Ҫ�ڿ���̨��ʾ������
        /// </summary>
        void Print();
    }
    /// <summary>
    /// ��������Ҫ��������л���ʽ�Ķ���
    /// </summary>
    public interface IJsonable : IPrintable
    {
        /// <summary>
        /// ���ؾ�����������л����Json
        /// </summary>
        string Json();
    }
    public interface IDetailable : IJsonable
    {
        /// <summary>
        /// ���ؾ�������������ϸ���л����Json
        /// </summary>
        string JsonDetail();
    }
}