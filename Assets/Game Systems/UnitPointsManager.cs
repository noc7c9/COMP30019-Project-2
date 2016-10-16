public class UnitPointsManager {

    int unusedUnitPoints;
    int maxUnitPoints;

    public UnitPointsManager(int unusedUnitPoints, int maxUnitPoints) {
        this.maxUnitPoints = maxUnitPoints;
        this.unusedUnitPoints = unusedUnitPoints;
    }

    public int GetUnusedPoints() {
        return unusedUnitPoints;
    }

    public int GetMaxPoints() {
        return maxUnitPoints;
    }

    public bool CanIncrement(int value=1) {
        return unusedUnitPoints <= maxUnitPoints - value;
    }

    public bool CanDecrement(int value=1) {
        return unusedUnitPoints >= value;
    }

    public void Increment(int value=1) {
        if (CanIncrement(value)) {
            unusedUnitPoints = unusedUnitPoints + value;
        }
    }

    public void Decrement(int value=1) {
        if (CanDecrement(value)) {
            unusedUnitPoints -= value;
        }
    }

}