using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuGen : MonoBehaviour
{
    List<string> ransoms = new List<string>
    {
        "Hello", "Niket", "Akshan", "Abhishek", "Anant", "apple","river","cloud","stone","mirror","lantern","forest","echo","spark","shadow","flame","garden","whisper","mountain","thread","circle","storm","feather","glass","bridge","dream","ocean","leaf","ember","tower","song","path","light","wind","flower","gate","sand","shell","horizon","drift","pulse","candle","veil","root","star","wave","key","door","map","book","clock","field","rain","snow","sun","moon","sky","dust","fire","ice","gold","silver","iron","rope","chain","mask","voice","heart","mind","soul","stone","tree","branch","seed","petal","wing","scale","horn","claw","fang","veil","mist","fog","echo","tide","current","flute","drum","bell","note","chord","verse","line","page","ink","quill","scroll","code","signal","pulse","thread","net","web","grid","spark","charge","bolt","circuit","gear","wheel","lever","spring","axis","core","edge","point","sphere","cube","prism","crystal","gem","pearl","jade","opal","ruby","sapphire","diamond","topaz","amber","coal","salt","grain","bread","milk","honey","fruit","meat","water","wine","ale","tea","coffee","sugar","spice","herb","root","stem","bud","bloom","dew","drop","river","lake","pond","sea","bay","gulf","cape","island","reef","delta","plain","hill","peak","ridge","valley","cave","mine","pit","well","spring","stream","brook","creek","channel","canal","harbor","port","dock","ship","boat","raft","sail","oar","anchor","compass","chart","flag","banner","crest","crown","throne","sword","shield","armor","helm","spear","bow","arrow","dagger","axe","hammer","staff","wand","orb","ring","amulet","charm","totem","idol","statue","figure","image","icon","symbol","sign","mark","letter","word","name","title","story","tale","legend","myth","fable","poem","song","chant","prayer","wish","hope","fear","joy","grief","love","hate","peace","war","life","death","birth","fate","destiny","chance","luck","fortune","doom","curse","blessing","gift","treasure","wealth","coin","note","bill","debt","loan","trade","market","shop","stall","cart","road","street","lane","alley","square","plaza","court","hall","house","home","hut","cabin","tent","camp","fort","castle","palace","temple","church","shrine","altar","grave","tomb","crypt","vault","cell","dungeon","prison","tower","keep","wall","gate","door","window","roof","floor","stairs","ladder","bridge","arch","pillar","column","beam","stone","brick","tile","wood","plank","board","nail","screw","bolt","nut","chain","rope","cord","line","thread","fiber","cloth","fabric","silk","wool","cotton","linen","leather","hide","fur","skin","bone","blood","flesh","meat","muscle","nerve","brain","heart","lung","eye","ear","hand","foot","arm","leg","head","face","mouth","nose","tongue","tooth","hair","beard","voice","cry","laugh","shout","call","scream","whisper","silence","sound","noise","music","tone","pitch","beat","rhythm","melody","harmony","verse","chorus","bridge","solo","band","choir","orchestra","drum","flute","horn","pipe","string","bow","key","note","scale","song","dance","step","move","turn","spin","jump","fall","rise","walk","run","ride","fly","swim","crawl","climb","dig","build","make","craft","forge","shape","form","mold","cast","paint","draw","write","read","speak","listen","watch","see","look","glance","stare","gaze","view","sight","vision","dream","thought","idea","plan","scheme","plot","goal","aim","task","work","job","duty","role","part","act","play","game","sport","race","match","fight","battle","war","peace","truce","deal","bond","link","tie","union","group","team","crowd","mob","army","host","tribe","clan","family","friend","foe","ally","enemy","king","queen","lord","lady","prince","princess","knight","squire","mage","wizard","witch","sorcerer","seer","prophet","sage","monk","priest","nun","angel","demon","spirit","ghost","shade","phantom","beast","creature","monster","giant","dragon","serpent","wolf","lion","bear","hawk","eagle","owl","cat","dog","horse","cow","pig","sheep","goat","fish","bird","insect","ant","bee","fly","worm","snake","frog","rat","mouse","bat","fox","deer","elk","moose","camel","zebra","giraffe","elephant","tiger","leopard","cheetah","panther","crocodile","shark","whale","dolphin","seal","otter","penguin","parrot","sparrow","crow","raven","swallow","crane","stork","duck","goose","swan","chicken","rooster","turkey","peacock","pigeon","dove","falcon","vulture","buzzard","lark","finch","robin","bluejay","cardinal","woodpecker","hummingbird","canary","seagull","pelican","albatross","ostrich","emu","kiwi","kangaroo","koala","panda","monkey","ape","gorilla","chimp","baboon","lemur","sloth","armadillo","hedgehog","porcupine","skunk","raccoon","beaver","badger","weasel","ferret","mole","platypus","anteater","aardvark","rhino","hippo","buffalo","bison","yak","ox","donkey","mule","camel","llama","alpaca","reindeer","caribou","boar","hare","rabbit","squirrel","chipmunk","hamster","guinea","parakeet","canary","finch","parrot","macaw","cockatoo","toucan","ibis","heron","egret","flamingo","phoenix"
    };
    public NoiseSettings mainNoise;
    public NoiseSettings biomeNoise;

    public TMP_InputField SeedCarrier;
    public void OnRandomSeed()
    {
        SeedCarrier.text = ransoms[(int)Random.Range(0, ransoms.Count+0.5f)];  
    } 

    public void GenerateWorld()
    {
        string mega = SeedCarrier.text;
        int sum = 0;
        int mult = 1;
        foreach(char c in mega)
        {
            sum += (int)c;
            mult *= (int)c;
            mult %= 1000000;
        }
        mainNoise.offset = new Vector2Int(sum, mult);
        biomeNoise.offset = new Vector2Int(sum + 170, mult + 180);
        SceneManager.LoadScene("SampleScene");

    }
}
