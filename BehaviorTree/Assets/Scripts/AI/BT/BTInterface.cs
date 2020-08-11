using System;

//Character和Article公共部分
public interface IBTActionOwner
{
    void SetOwner(BaseSprite owner);

    BaseSprite GetOwner();
}

