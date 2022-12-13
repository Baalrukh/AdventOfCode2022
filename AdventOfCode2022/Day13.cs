using AdventOfCode2022.Utils;

namespace AdventOfCode2022;

public class Day13 : Exercise
{
    public long ExecutePart1(string[] lines)
    {
        var batches = lines.Batch(x => x.Length == 0, true);
        return batches.Select((x, i) => (i + 1, IsInRightOrder(ParseNode(x[0]), ParseNode(x[1]))))
            .Where(x => x.Item2)
            .Sum(x => x.Item1);
    }

    public long ExecutePart2(string[] lines)
    {
        var separator2 = new ListNode(new[] {new ListNode(new [] {new NumberNode(2)})});
        var separator6 = new ListNode(new[] {new ListNode(new [] {new NumberNode(6)})});
        var nodes = lines.Where(x => x != String.Empty).Select(ParseNode).Concat(new PacketNode[] {separator2, separator6}).ToList();
        nodes.Sort();

        var indexOf2 = nodes.IndexOf(separator2);
        var indexOf6 = nodes.IndexOf(separator6);

        return (indexOf2 + 1) * (indexOf6 + 1);
    }

    public static bool IsInRightOrder(PacketNode left, PacketNode right)
    {
        return left.CompareTo(right) < 0;
    }

    public static PacketNode ParseNode(string line)
    {
        int i = 1;
        return ParseNodeList(line, ref i);
    }

    private static ListNode ParseNodeList(string text, ref int i)
    {
        List<PacketNode> nodes = new List<PacketNode>();
        while (text[i] != ']')
        {
            if (text[i] == '[')
            {
                i++;
                nodes.Add(ParseNodeList(text, ref i));
            }
            else if (char.IsDigit(text[i]))
            {
                nodes.Add(ParseNumberNode(text, ref i));
            }

            if (text[i] == ',')
            {
                i++; // ,
            }
        }
        i++;
        return new ListNode(nodes);
    }

    private static PacketNode ParseNumberNode(string text, ref int i)
    {
        int start = i;
        i++;
        while (char.IsDigit(text[i]))
        {
            i++;
        }

        return new NumberNode(int.Parse(text[start..i]));
    }


    public interface PacketNode : IComparable<PacketNode>
    {
        // int CompareTo(PacketNode other);
    }

    public record NumberNode(int Value) : PacketNode
    {
        public int CompareTo(PacketNode other)
        {
            if (other is NumberNode otherNumber)
            {
                return Value.CompareTo(otherNumber.Value);
            }

            return new ListNode(new [] {this}).CompareTo(other);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class ListNode : PacketNode
    {
        public readonly List<PacketNode> Nodes;

        public ListNode(IEnumerable<PacketNode> nodes)
        {
            Nodes = nodes.ToList();
        }

        protected bool Equals(ListNode other)
        {
            return Nodes.SequenceEqual(other.Nodes);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((ListNode) obj);
        }

        public override int GetHashCode()
        {
            return Nodes.GetHashCode();
        }

        public int CompareTo(PacketNode other)
        {
            if (other is NumberNode numberNode)
            {
                other = new ListNode(new[] {numberNode});
            }

            if (other is ListNode otherList)
            {
                for (int i = 0; i < Math.Min(Nodes.Count, otherList.Nodes.Count); i++)
                {
                    var elementCompareResult = Nodes[i].CompareTo(otherList.Nodes[i]);
                    if (elementCompareResult != 0)
                    {
                        return elementCompareResult;
                    }
                }

                if (Nodes.Count == otherList.Nodes.Count)
                {
                    return 0;
                }

                return Nodes.Count < otherList.Nodes.Count ? -1 : 1;
            }

            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"[{string.Join(", ", Nodes)}]";
        }
    }
}
