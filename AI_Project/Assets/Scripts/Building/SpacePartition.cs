using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacePartition {

    private SubSpace rootSubSpace;
    private int minSize, maxSize;

    public SpacePartition(RectInt area, int minSize, int maxSize, int seed) {
        Random.InitState(seed);
        rootSubSpace = new SubSpace(area);
        this.minSize = minSize;
        this.maxSize = maxSize;
        Partition(rootSubSpace);
    }

    public void Partition(SubSpace subArea) {
        if (subArea.IsLeaf()) {
            // if the sub-dungeon is too large
            if (subArea.rect.width > maxSize || subArea.rect.height > maxSize || Random.value < 0.25f) {
                if (subArea.Split(minSize, maxSize)) {
                    Partition(subArea.left);
                    Partition(subArea.right);
                }
            }
        }
    }

    public List<RectInt> GetSpaces() {
        return rootSubSpace.GetSpaces();
    }
}



public class SubSpace {

    public SubSpace left, right;
    public RectInt rect;
    public int debugId;

    public SubSpace(RectInt area) {
        rect = area;
    }

    public bool IsLeaf() {
        return left == null && right == null;
    }

    public bool Split(int minSize, int maxSize) {
        if (minSize > maxSize || minSize <= 0) { return false; }
        if (!IsLeaf()) { return false; }

        bool canSplitH = true, canSplitV = true;
        if (rect.height / 2 < minSize) { canSplitH = false; }
        if (rect.width / 2 < minSize) { canSplitV = false; }

        if (!(canSplitH || canSplitV)) { return false; } // can't split at all

        bool splitH;
        if (canSplitH && canSplitV) {
            // choose a vertical or horizontal split depending on the proportions
            // i.e. if too wide split vertically, or too long horizontally,
            // or if nearly square choose vertical or horizontal at random
            if (rect.width / rect.height >= 1.25) {
                splitH = false;
            }
            else if (rect.height / rect.width >= 1.25) {
                splitH = true;
            }
            else {
                splitH = Random.Range(0.0f, 1.0f) > 0.5f;
            }
        }
        else if (canSplitH) {
            splitH = true;
        }
        else {
            splitH = false;
        }

        if (splitH) {
            // split so that the resulting sub-spaces heights are not too small
            // (since we are splitting horizontally)
            int split = Random.Range(minSize, rect.height - minSize);

            left = new SubSpace(new RectInt(rect.x, rect.y, rect.width, split));
            right = new SubSpace(new RectInt(rect.x, rect.y + split, rect.width, rect.height - split));
        }
        else {
            // split so that the resulting sub-spaces widths are not too small
            // (since we are splitting vertically)
            int split = Random.Range(minSize, (int)(rect.width - minSize));

            left = new SubSpace(new RectInt(rect.x, rect.y, split, rect.height));
            right = new SubSpace(new RectInt(rect.x + split, rect.y, rect.width - split, rect.height));
        }

        return true;
    }

    public List<RectInt> GetSpaces() {
        if (IsLeaf()) {
            return new List<RectInt> { rect };
        }
        else {
            List<RectInt> lList = left?.GetSpaces();
            List<RectInt> rList = right?.GetSpaces();
            lList.AddRange(rList);
            return lList;
        }
    }
}
