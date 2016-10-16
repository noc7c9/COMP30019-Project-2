public class BuildPointsManager {

    int buildPoints;

    public BuildPointsManager(int buildPoints) {
        this.buildPoints = buildPoints;
    }

    public int GetPoints() {
        return buildPoints;
    }

    public bool CanIncrement(int value=1) {
        return true;
    }

    public bool CanDecrement(int value=1) {
        return buildPoints >= value;
    }

    public void Increment(int value=1) {
        if (CanIncrement(value)) {
            buildPoints += value;
        }
    }

    public void Decrement(int value=1) {
        if (CanDecrement(value)) {
            buildPoints -= value;
        }
    }

}