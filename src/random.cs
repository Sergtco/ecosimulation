using System;


namespace simulation;

static class Rand{
    static Random random = new Random(Guid.NewGuid().GetHashCode());
    public static int get(int start, int end) {
        return random.Next(start, end);
    }
}
