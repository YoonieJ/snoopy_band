// BandMember.cs
// Represents a single member of the band, which consists of a dog and an instrument.
// Attach the instrument to the dog, update the dog's and instrument's animations,
// and draw the dog (the instrument will be drawn as part of the dog's hierarchy).

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace snoopy_band;

public class BandMember
{
    public string Name { get; }
    public Dog Dog { get; }
    public InstrumentBase Instrument { get; }

    // Set Depth for band layout positions
    public float Depth { get; set; }

    public BandMember(Dog dog, InstrumentBase instrument, string name)
    {
        Dog = dog;
        Instrument = instrument;
        Name = name;
    }

    // Attach the instrument to the dog so it moves with the dog and animates together
    public void AttachInstrumentToDog()
    {
        Instrument.AttachToDog(Dog);
    }

    // Update both the dog and the instrument animations
    public void Update(GameTime gt)
    {
        Dog.Update(gt);
        Instrument.Update(gt);
    }

    public void Draw(SpriteBatch sb)
    {
        Dog.Draw(sb);
    }
}