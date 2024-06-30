using ShortGenerator.Reddit;
using ShortGenerator.Video;

Console.WriteLine("Short King Generator");


/*

InputData inputData = new InputData(
    "Test",
    "./video/bg/1.mp4",
    "",
    VideoCategory.Entertainment,
    false
);

RedditPost redditPost = new RedditPost(
    "AskReddit",
    "Goddess-flirt548",
    "Who is someone everyone thought was crazy but turned out to be right?",
    "",
    new RedditAnswer[]
    {
        new RedditAnswer("WildBad7298", $@"
Rick Rescorla. He was director of security at Morgan Stanley, which had its offices in the South Tower of the World Trade Center. Rescorla was convinced that the building was vulnerable to a terror attack, and insisted on emergency plans and evacuation drills. His higher-ups viewed them as a nuisance and waste of time.

On September 11, when the North Tower was hit by an airplane, the executives at Morgan Stanley told their employees to stay put. Rescorla phoned a friend, saying, {"\""}The dumb sons of bitches told me not to evacuate. They said it's just Building One. I told them I'm getting my people the fuck out of here.{"\""} He led around 2,700 employees downstairs to safety, and died in the building's collapse after going back in to lead more people out.

https://en.m.wikipedia.org/wiki/Rick_Rescorla

EDIT: My mistake, it was the New York Port Authority that urged workers to remain at their desks.
"),
        new RedditAnswer("tommytraddles", $@"
Kotoku Wamura, for sure.

He was mayor of the Japanese town of Fudai for several decades, starting just after WWII up into the 1980s.

He was aware that Fudai had been flattened in the past by tsunamis, only to be rebuilt in the same place. He learned there was nothing protecting his town. So, he ordered the construction of a state-of-the-art seawall. It was very expensive, and laughed at as a folly. Wamura was personally attacked as crazy and wasteful in the national and even international press. He died in 1997.

In 2011, when the Tōhoku earthquake and tsunami struck Japan, it killed roughly 20,000 people.

But the Fudai seawall held, and the town escaped almost untouched. 3,000 people were saved.
"),
        new RedditAnswer("closequartersbrewing", $@"
In 1957 Manitoba Premier Duff Roblin authorized a flood control waterway through Winnipeg. The project was the second largest earth-moving project in the world, after the Panema Canal (even more then the Suez canal). The entire province had a population of 900,000.

It was completed on time, and under budget, but he got skewered for it as being unnecessary. It got branded {"\""}Duff's Ditch{"\""}, and “approximating the building of the pyramids of Egypt in terms of usefulness.”

Since then, it's saved the city from several floods, saving over 40 billion. It was designated a national historic cite as an outstanding engineering achievement both in terms of function and impact.
"),
    }
);

VideoGenerator.GenerateReddit(inputData, redditPost, true);

*/

InputData inputData = new InputData(
    "Test",
    "./video/bg/1.mp4",
    "",
    VideoCategory.Entertainment,
    false
);

RedditPost redditPost = new RedditPost(
    "AmItheAsshole",
    "StruggleBusDriver12",
    "AITA for not naming my children after my in-laws?",
    $@"
My husband and I (33M and 31F respectively) have a 3 y.o. son and are trying for a second child soon. My father passed away the day I found out I was pregnant with my son, so my husband and I agreed to give my dad’s middle name “Hayden” to our son. This caused a bit of a fuss with my FIL, who said he was hoping I would honor him in naming our child, but I repeatedly told him the names my DH and I picked for our children would not mesh well with any of his names. I didn’t mention that we didn’t want to carry the “James” name tradition on anymore nor did I like the name “Martin”. Now that my DH and I are trying for a second child, the argument has come up again about bestowing a “family name” to honor my FIL upon our second child should it be another boy. We already have another name set picked out for another boy, and this one honors my grandfather who passed before I was born. The name we have chosen flows very well with my grandfather’s name, and my DH agrees it would be a great choice. My FIL made a comment to me about “having to die before he’d get a child named after him”, to which I made it abundantly clear that neither I, my DH, nor his other son and his partner have any obligation to name any of our children after him. This has caused a huge rift in the family, and my MIL has pleaded with me to reconsider and allow FIL the pride of having a child named after him. I am standing my ground and keeping all the names we have picked as they are. AITA?
",
    new RedditAnswer[]
    {
        new RedditAnswer("alien_overlord_1001", $@"
NTA why does he assume he would be honored over your family? How presumptuous of him.

They named their own kids, they don’t get to name yours. Stand your ground.
"),
    }
);

VideoGenerator.GenerateReddit(inputData, redditPost, true);
