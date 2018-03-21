// Save Monaco's amd require and restore Node's require
var amdRequire = global.require;
global.require = nodeRequire;

// require node modules before loader.js comes in
var path = require('path');
function uriFromPath(_path) {
  var pathName = path.resolve(_path).replace(/\\/g, '/');
  if (pathName.length > 0 && pathName.charAt(0) !== '/') {
    pathName = '/' + pathName;
  }
  return encodeURI('file://' + pathName);
}
amdRequire.config({
  baseUrl: uriFromPath(path.join(__dirname, '../node_modules/monaco-editor/min'))
});
// workaround monaco-css not understanding the environment
self.module = undefined;
// workaround monaco-typescript not understanding the environment
self.process.browser = true;
amdRequire(['vs/editor/editor.main'], function () {

  monaco.languages.register({
    id: 'arm'
  });
  monaco.languages.setMonarchTokensProvider('arm', {
    // Set defaultToken to invalid to see what you do not tokenize yet
    defaultToken: 'invalid',

    ignoreCase: true,

    brackets: [
      ['{', '}', 'delimiter.curly'],
      ['[', ']', 'delimiter.square'],
      ['(', ')', 'delimiter.parenthesis']
    ],

    operators: [
      '=', '>', '!'
    ],

    keywords: [
      'ADC', 'ADCAL', 'ADCEQ', 'ADCGE', 'ADCGT', 'ADCHI', 'ADCHS', 'ADCLE', 'ADCLO', 'ADCLS', 'ADCLT', 'ADCMI', 'ADCNE', 'ADCNV', 'ADCPL', 'ADCS', 'ADCSAL', 'ADCSEQ', 'ADCSGE', 'ADCSGT', 'ADCSHI', 'ADCSHS', 'ADCSLE', 'ADCSLO', 'ADCSLS', 'ADCSLT', 'ADCSMI', 'ADCSNE', 'ADCSNV', 'ADCSPL', 'ADCSVC', 'ADCSVS', 'ADCVC', 'ADCVS', 'ADD', 'ADDAL', 'ADDEQ', 'ADDGE', 'ADDGT', 'ADDHI', 'ADDHS', 'ADDLE', 'ADDLO', 'ADDLS', 'ADDLT', 'ADDMI', 'ADDNE', 'ADDNV', 'ADDPL', 'ADDS', 'ADDSAL', 'ADDSEQ', 'ADDSGE', 'ADDSGT', 'ADDSHI', 'ADDSHS', 'ADDSLE', 'ADDSLO', 'ADDSLS', 'ADDSLT', 'ADDSMI', 'ADDSNE', 'ADDSNV', 'ADDSPL', 'ADDSVC', 'ADDSVS', 'ADDVC', 'ADDVS', 'ADR', 'ADRAL', 'ADREQ', 'ADRGE', 'ADRGT', 'ADRHI', 'ADRHS', 'ADRLE', 'ADRLO', 'ADRLS', 'ADRLT', 'ADRMI', 'ADRNE', 'ADRNV', 'ADRPL', 'ADRVC', 'ADRVS', 'AND', 'ANDAL', 'ANDEQ', 'ANDGE', 'ANDGT',
      'ANDHI', 'ANDHS', 'ANDLE', 'ANDLO', 'ANDLS', 'ANDLT', 'ANDMI', 'ANDNE', 'ANDNV', 'ANDPL', 'ANDS', 'ANDSAL', 'ANDSEQ', 'ANDSGE',
      'ANDSGT', 'ANDSHI', 'ANDSHS', 'ANDSLE', 'ANDSLO', 'ANDSLS', 'ANDSLT', 'ANDSMI', 'ANDSNE', 'ANDSNV', 'ANDSPL', 'ANDSVC', 'ANDSVS', 'ANDVC', 'ANDVS', 'ASR', 'ASRAL', 'ASREQ', 'ASRGE', 'ASRGT', 'ASRHI', 'ASRHS', 'ASRLE', 'ASRLO', 'ASRLS', 'ASRLT', 'ASRMI', 'ASRNE', 'ASRNV', 'ASRPL', 'ASRS', 'ASRSAL', 'ASRSEQ', 'ASRSGE', 'ASRSGT', 'ASRSHI', 'ASRSHS', 'ASRSLE', 'ASRSLO', 'ASRSLS', 'ASRSLT', 'ASRSMI', 'ASRSNE', 'ASRSNV', 'ASRSPL', 'ASRSVC', 'ASRSVS', 'ASRVC', 'ASRVS', 'B', 'BAL', 'BEQ', 'BGE', 'BGT', 'BHI', 'BHS', 'BIC', 'BICAL', 'BICEQ', 'BICGE', 'BICGT', 'BICHI', 'BICHS', 'BICLE', 'BICLO', 'BICLS', 'BICLT', 'BICMI', 'BICNE', 'BICNV', 'BICPL', 'BICS', 'BICSAL', 'BICSEQ', 'BICSGE', 'BICSGT', 'BICSHI', 'BICSHS', 'BICSLE', 'BICSLO', 'BICSLS', 'BICSLT', 'BICSMI', 'BICSNE', 'BICSNV', 'BICSPL', 'BICSVC', 'BICSVS', 'BICVC', 'BICVS', 'BL', 'BLAL', 'BLE', 'BLEQ', 'BLGE', 'BLGT', 'BLHI', 'BLHS', 'BLLE', 'BLLO', 'BLLS', 'BLLT', 'BLMI', 'BLNE', 'BLNV', 'BLO', 'BLPL', 'BLS', 'BLT', 'BLVC', 'BLVS', 'BMI', 'BNE', 'BNV', 'BPL', 'BVC', 'BVS', 'CMN', 'CMNAL', 'CMNEQ', 'CMNGE', 'CMNGT', 'CMNHI', 'CMNHS', 'CMNLE', 'CMNLO', 'CMNLS', 'CMNLT', 'CMNMI', 'CMNNE',
      'CMNNV', 'CMNPL', 'CMNVC', 'CMNVS', 'CMP', 'CMPAL', 'CMPEQ', 'CMPGE', 'CMPGT', 'CMPHI', 'CMPHS', 'CMPLE', 'CMPLO', 'CMPLS', 'CMPLT', 'CMPMI', 'CMPNE', 'CMPNV', 'CMPPL', 'CMPVC', 'CMPVS', 'DCD', 'DCDAL', 'DCDEQ', 'DCDGE', 'DCDGT', 'DCDHI', 'DCDHS', 'DCDLE', 'DCDLO', 'DCDLS', 'DCDLT', 'DCDMI', 'DCDNE', 'DCDNV', 'DCDPL',
      'DCDVC', 'DCDVS', 'END', 'ENDAL', 'ENDEQ', 'ENDGE', 'ENDGT', 'ENDHI', 'ENDHS', 'ENDLE', 'ENDLO', 'ENDLS', 'ENDLT', 'ENDMI', 'ENDNE', 'ENDNV', 'ENDPL', 'ENDVC', 'ENDVS', 'EOR', 'EORAL', 'EOREQ', 'EORGE', 'EORGT', 'EORHI', 'EORHS', 'EORLE', 'EORLO', 'EORLS', 'EORLT', 'EORMI', 'EORNE', 'EORNV', 'EORPL', 'EORS', 'EORSAL',
      'EORSEQ', 'EORSGE', 'EORSGT', 'EORSHI', 'EORSHS', 'EORSLE', 'EORSLO', 'EORSLS', 'EORSLT', 'EORSMI', 'EORSNE', 'EORSNV', 'EORSPL', 'EORSVC', 'EORSVS', 'EORVC', 'EORVS', 'EQU', 'EQUAL', 'EQUEQ', 'EQUGE', 'EQUGT', 'EQUHI', 'EQUHS', 'EQULE', 'EQULO', 'EQULS',
      'EQULT', 'EQUMI', 'EQUNE', 'EQUNV', 'EQUPL', 'EQUVC', 'EQUVS', 'FILL', 'FILLAL', 'FILLEQ', 'FILLGE', 'FILLGT', 'FILLHI', 'FILLHS', 'FILLLE', 'FILLLO', 'FILLLS', 'FILLLT', 'FILLMI', 'FILLNE', 'FILLNV', 'FILLPL', 'FILLVC', 'FILLVS', 'LDM', 'LDMAL', 'LDMDB',
      'LDMDBAL', 'LDMDBEQ', 'LDMDBGE', 'LDMDBGT', 'LDMDBHI', 'LDMDBHS', 'LDMDBLE', 'LDMDBLO', 'LDMDBLS', 'LDMDBLT', 'LDMDBMI', 'LDMDBNE', 'LDMDBNV', 'LDMDBPL', 'LDMDBVC', 'LDMDBVS', 'LDMEA', 'LDMEAAL', 'LDMEAEQ', 'LDMEAGE', 'LDMEAGT', 'LDMEAHI', 'LDMEAHS', 'LDMEALE', 'LDMEALO', 'LDMEALS', 'LDMEALT', 'LDMEAMI', 'LDMEANE', 'LDMEANV', 'LDMEAPL', 'LDMEAVC', 'LDMEAVS', 'LDMED', 'LDMEDAL', 'LDMEDEQ', 'LDMEDGE', 'LDMEDGT', 'LDMEDHI', 'LDMEDHS', 'LDMEDLE', 'LDMEDLO', 'LDMEDLS', 'LDMEDLT', 'LDMEDMI', 'LDMEDNE', 'LDMEDNV', 'LDMEDPL', 'LDMEDVC', 'LDMEDVS', 'LDMEQ', 'LDMFA', 'LDMFAAL', 'LDMFAEQ', 'LDMFAGE', 'LDMFAGT', 'LDMFAHI', 'LDMFAHS', 'LDMFALE', 'LDMFALO', 'LDMFALS', 'LDMFALT', 'LDMFAMI', 'LDMFANE', 'LDMFANV', 'LDMFAPL', 'LDMFAVC', 'LDMFAVS', 'LDMFD', 'LDMFDAL', 'LDMFDEQ', 'LDMFDGE', 'LDMFDGT', 'LDMFDHI', 'LDMFDHS', 'LDMFDLE', 'LDMFDLO', 'LDMFDLS', 'LDMFDLT', 'LDMFDMI', 'LDMFDNE', 'LDMFDNV', 'LDMFDPL', 'LDMFDVC', 'LDMFDVS', 'LDMGE', 'LDMGT', 'LDMHI', 'LDMHS', 'LDMIA', 'LDMIAAL', 'LDMIAEQ', 'LDMIAGE', 'LDMIAGT', 'LDMIAHI', 'LDMIAHS', 'LDMIALE', 'LDMIALO', 'LDMIALS', 'LDMIALT', 'LDMIAMI', 'LDMIANE', 'LDMIANV', 'LDMIAPL', 'LDMIAVC', 'LDMIAVS', 'LDMLE', 'LDMLO', 'LDMLS', 'LDMLT', 'LDMMI', 'LDMNE', 'LDMNV', 'LDMPL', 'LDMVC', 'LDMVS', 'LDR', 'LDRAL', 'LDRB', 'LDRBAL', 'LDRBEQ',
      'LDRBGE', 'LDRBGT', 'LDRBHI', 'LDRBHS', 'LDRBLE', 'LDRBLO', 'LDRBLS', 'LDRBLT', 'LDRBMI', 'LDRBNE', 'LDRBNV', 'LDRBPL', 'LDRBVC', 'LDRBVS', 'LDREQ', 'LDRGE', 'LDRGT', 'LDRHI', 'LDRHS', 'LDRLE', 'LDRLO', 'LDRLS', 'LDRLT', 'LDRMI', 'LDRNE', 'LDRNV', 'LDRPL', 'LDRVC', 'LDRVS', 'LSL', 'LSLAL', 'LSLEQ', 'LSLGE', 'LSLGT', 'LSLHI', 'LSLHS', 'LSLLE', 'LSLLO', 'LSLLS', 'LSLLT', 'LSLMI', 'LSLNE', 'LSLNV', 'LSLPL', 'LSLS', 'LSLSAL', 'LSLSEQ', 'LSLSGE', 'LSLSGT', 'LSLSHI', 'LSLSHS', 'LSLSLE', 'LSLSLO', 'LSLSLS', 'LSLSLT', 'LSLSMI', 'LSLSNE', 'LSLSNV', 'LSLSPL', 'LSLSVC', 'LSLSVS',
      'LSLVC', 'LSLVS', 'LSR', 'LSRAL', 'LSREQ', 'LSRGE', 'LSRGT', 'LSRHI', 'LSRHS', 'LSRLE', 'LSRLO', 'LSRLS', 'LSRLT', 'LSRMI', 'LSRNE', 'LSRNV', 'LSRPL', 'LSRS', 'LSRSAL', 'LSRSEQ', 'LSRSGE', 'LSRSGT', 'LSRSHI', 'LSRSHS', 'LSRSLE', 'LSRSLO', 'LSRSLS', 'LSRSLT', 'LSRSMI', 'LSRSNE', 'LSRSNV', 'LSRSPL', 'LSRSVC', 'LSRSVS', 'LSRVC', 'LSRVS', 'MOV', 'MOVAL', 'MOVEQ', 'MOVGE', 'MOVGT', 'MOVHI', 'MOVHS', 'MOVLE', 'MOVLO', 'MOVLS', 'MOVLT', 'MOVMI', 'MOVNE', 'MOVNV', 'MOVPL', 'MOVS', 'MOVSAL', 'MOVSEQ', 'MOVSGE', 'MOVSGT', 'MOVSHI', 'MOVSHS', 'MOVSLE', 'MOVSLO', 'MOVSLS', 'MOVSLT', 'MOVSMI', 'MOVSNE', 'MOVSNV', 'MOVSPL', 'MOVSVC', 'MOVSVS', 'MOVVC', 'MOVVS', 'MVN', 'MVNAL', 'MVNEQ', 'MVNGE', 'MVNGT', 'MVNHI', 'MVNHS', 'MVNLE', 'MVNLO', 'MVNLS', 'MVNLT', 'MVNMI', 'MVNNE', 'MVNNV', 'MVNPL', 'MVNS', 'MVNSAL', 'MVNSEQ', 'MVNSGE', 'MVNSGT', 'MVNSHI', 'MVNSHS', 'MVNSLE', 'MVNSLO', 'MVNSLS', 'MVNSLT', 'MVNSMI', 'MVNSNE', 'MVNSNV', 'MVNSPL', 'MVNSVC', 'MVNSVS', 'MVNVC', 'MVNVS', 'ORR', 'ORRAL', 'ORREQ', 'ORRGE', 'ORRGT', 'ORRHI', 'ORRHS', 'ORRLE', 'ORRLO', 'ORRLS', 'ORRLT', 'ORRMI', 'ORRNE', 'ORRNV', 'ORRPL', 'ORRS', 'ORRSAL', 'ORRSEQ', 'ORRSGE', 'ORRSGT', 'ORRSHI', 'ORRSHS', 'ORRSLE', 'ORRSLO', 'ORRSLS', 'ORRSLT',
      'ORRSMI', 'ORRSNE', 'ORRSNV', 'ORRSPL', 'ORRSVC', 'ORRSVS', 'ORRVC', 'ORRVS', 'ROR', 'RORAL', 'ROREQ', 'RORGE', 'RORGT', 'RORHI', 'RORHS', 'RORLE', 'RORLO', 'RORLS', 'RORLT', 'RORMI', 'RORNE', 'RORNV', 'RORPL', 'RORS', 'RORSAL', 'RORSEQ', 'RORSGE', 'RORSGT', 'RORSHI', 'RORSHS', 'RORSLE', 'RORSLO', 'RORSLS', 'RORSLT', 'RORSMI', 'RORSNE', 'RORSNV', 'RORSPL', 'RORSVC', 'RORSVS', 'RORVC', 'RORVS', 'RRX', 'RRXAL', 'RRXEQ', 'RRXGE', 'RRXGT', 'RRXHI', 'RRXHS', 'RRXLE', 'RRXLO', 'RRXLS', 'RRXLT', 'RRXMI', 'RRXNE',
      'RRXNV', 'RRXPL', 'RRXS', 'RRXSAL', 'RRXSEQ', 'RRXSGE', 'RRXSGT', 'RRXSHI', 'RRXSHS', 'RRXSLE', 'RRXSLO', 'RRXSLS', 'RRXSLT', 'RRXSMI', 'RRXSNE', 'RRXSNV', 'RRXSPL', 'RRXSVC', 'RRXSVS', 'RRXVC', 'RRXVS', 'RSB', 'RSBAL', 'RSBEQ', 'RSBGE', 'RSBGT', 'RSBHI',
      'RSBHS', 'RSBLE', 'RSBLO', 'RSBLS', 'RSBLT', 'RSBMI', 'RSBNE', 'RSBNV', 'RSBPL', 'RSBS', 'RSBSAL', 'RSBSEQ', 'RSBSGE', 'RSBSGT', 'RSBSHI', 'RSBSHS', 'RSBSLE', 'RSBSLO', 'RSBSLS', 'RSBSLT', 'RSBSMI', 'RSBSNE', 'RSBSNV', 'RSBSPL', 'RSBSVC', 'RSBSVS', 'RSBVC', 'RSBVS', 'RSC', 'RSCAL', 'RSCEQ', 'RSCGE', 'RSCGT', 'RSCHI', 'RSCHS', 'RSCLE', 'RSCLO', 'RSCLS', 'RSCLT', 'RSCMI', 'RSCNE', 'RSCNV', 'RSCPL', 'RSCS', 'RSCSAL', 'RSCSEQ', 'RSCSGE', 'RSCSGT',
      'RSCSHI', 'RSCSHS', 'RSCSLE', 'RSCSLO', 'RSCSLS', 'RSCSLT', 'RSCSMI', 'RSCSNE', 'RSCSNV', 'RSCSPL', 'RSCSVC', 'RSCSVS', 'RSCVC', 'RSCVS', 'SBC', 'SBCAL', 'SBCEQ', 'SBCGE', 'SBCGT', 'SBCHI', 'SBCHS', 'SBCLE', 'SBCLO', 'SBCLS', 'SBCLT', 'SBCMI', 'SBCNE', 'SBCNV', 'SBCPL', 'SBCS', 'SBCSAL', 'SBCSEQ', 'SBCSGE', 'SBCSGT', 'SBCSHI', 'SBCSHS', 'SBCSLE', 'SBCSLO', 'SBCSLS', 'SBCSLT', 'SBCSMI', 'SBCSNE', 'SBCSNV', 'SBCSPL', 'SBCSVC', 'SBCSVS', 'SBCVC',
      'SBCVS', 'STM', 'STMAL', 'STMDB', 'STMDBAL', 'STMDBEQ', 'STMDBGE', 'STMDBGT', 'STMDBHI', 'STMDBHS', 'STMDBLE', 'STMDBLO', 'STMDBLS', 'STMDBLT', 'STMDBMI', 'STMDBNE', 'STMDBNV', 'STMDBPL', 'STMDBVC', 'STMDBVS', 'STMEA', 'STMEAAL', 'STMEAEQ', 'STMEAGE', 'STMEAGT', 'STMEAHI', 'STMEAHS', 'STMEALE', 'STMEALO', 'STMEALS', 'STMEALT', 'STMEAMI', 'STMEANE', 'STMEANV', 'STMEAPL', 'STMEAVC',
      'STMEAVS', 'STMED', 'STMEDAL', 'STMEDEQ', 'STMEDGE', 'STMEDGT',
      'STMEDHI', 'STMEDHS', 'STMEDLE', 'STMEDLO', 'STMEDLS', 'STMEDLT', 'STMEDMI', 'STMEDNE', 'STMEDNV', 'STMEDPL', 'STMEDVC', 'STMEDVS', 'STMEQ', 'STMFA', 'STMFAAL', 'STMFAEQ', 'STMFAGE', 'STMFAGT', 'STMFAHI', 'STMFAHS', 'STMFALE', 'STMFALO', 'STMFALS', 'STMFALT', 'STMFAMI', 'STMFANE', 'STMFANV', 'STMFAPL', 'STMFAVC', 'STMFAVS', 'STMFD', 'STMFDAL', 'STMFDEQ', 'STMFDGE', 'STMFDGT', 'STMFDHI', 'STMFDHS', 'STMFDLE', 'STMFDLO', 'STMFDLS', 'STMFDLT', 'STMFDMI', 'STMFDNE', 'STMFDNV', 'STMFDPL', 'STMFDVC', 'STMFDVS', 'STMGE', 'STMGT', 'STMHI', 'STMHS', 'STMIA', 'STMIAAL', 'STMIAEQ', 'STMIAGE', 'STMIAGT', 'STMIAHI', 'STMIAHS', 'STMIALE', 'STMIALO', 'STMIALS', 'STMIALT', 'STMIAMI', 'STMIANE', 'STMIANV', 'STMIAPL', 'STMIAVC', 'STMIAVS', 'STMLE', 'STMLO', 'STMLS', 'STMLT',
      'STMMI', 'STMNE', 'STMNV', 'STMPL', 'STMVC', 'STMVS', 'STR', 'STRAL', 'STRB', 'STRBAL', 'STRBEQ', 'STRBGE', 'STRBGT', 'STRBHI',
      'STRBHS', 'STRBLE', 'STRBLO', 'STRBLS', 'STRBLT', 'STRBMI', 'STRBNE', 'STRBNV', 'STRBPL', 'STRBVC', 'STRBVS', 'STREQ', 'STRGE',
      'STRGT', 'STRHI', 'STRHS', 'STRLE', 'STRLO', 'STRLS', 'STRLT', 'STRMI', 'STRNE', 'STRNV', 'STRPL', 'STRVC', 'STRVS', 'SUB', 'SUBAL', 'SUBEQ', 'SUBGE', 'SUBGT', 'SUBHI', 'SUBHS', 'SUBLE', 'SUBLO', 'SUBLS', 'SUBLT', 'SUBMI', 'SUBNE', 'SUBNV', 'SUBPL', 'SUBS', 'SUBSAL', 'SUBSEQ', 'SUBSGE', 'SUBSGT', 'SUBSHI', 'SUBSHS', 'SUBSLE', 'SUBSLO', 'SUBSLS', 'SUBSLT', 'SUBSMI', 'SUBSNE', 'SUBSNV', 'SUBSPL', 'SUBSVC', 'SUBSVS', 'SUBVC', 'SUBVS', 'TEQ', 'TEQAL', 'TEQEQ', 'TEQGE', 'TEQGT', 'TEQHI', 'TEQHS', 'TEQLE', 'TEQLO', 'TEQLS', 'TEQLT', 'TEQMI', 'TEQNE', 'TEQNV', 'TEQPL', 'TEQS', 'TEQSAL', 'TEQSEQ', 'TEQSGE', 'TEQSGT', 'TEQSHI', 'TEQSHS', 'TEQSLE', 'TEQSLO', 'TEQSLS', 'TEQSLT', 'TEQSMI', 'TEQSNE', 'TEQSNV', 'TEQSPL', 'TEQSVC', 'TEQSVS', 'TEQVC', 'TEQVS', 'TST', 'TSTAL', 'TSTEQ', 'TSTGE', 'TSTGT', 'TSTHI', 'TSTHS', 'TSTLE', 'TSTLO', 'TSTLS', 'TSTLT', 'TSTMI', 'TSTNE', 'TSTNV', 'TSTPL', 'TSTS',
      'TSTSAL', 'TSTSEQ', 'TSTSGE', 'TSTSGT', 'TSTSHI', 'TSTSHS', 'TSTSLE', 'TSTSLO', 'TSTSLS', 'TSTSLT', 'TSTSMI', 'TSTSNE', 'TSTSNV', 'TSTSPL', 'TSTSVC', 'TSTSVS', 'TSTVC', 'TSTVS'
    ],

    registers: [
      'R0','R1','R2','R3','R4','R5','R6','R7','R8','R9','R10','R11','R12',
      'R13', 'R14', 'R15', 'SP', 'LR', 'SP'
    ],

    // we include these common regular expressions
    symbols: /[=><!~?:&|+\-*\/\^%]+/,

    // C# style strings
    escapes: /\\(?:[abfnrtv\\"']|x[0-9A-Fa-f]{1,4}|u[0-9A-Fa-f]{4}|U[0-9A-Fa-f]{8})/,

    // The main tokenizer for our languages
    tokenizer: {
      root: [
        // identifiers and keywords
        [/[a-z_$][\w$]*/, {
          cases: {
            '@keywords': 'keyword',
            '@registers': 'register',
            '@default': 'identifier',
          }
        }],

        // whitespace
        { include: '@whitespace' },

        // delimiters and operators
        [/[{}()\[\]]/, '@brackets'],
        [/[<>](?!@symbols)/, '@brackets'],
        [/@symbols/, {
          cases: {
            '@operators': 'operator',
            '@default': ''
          }
        }],

        // @ annotations.
        // As an example, we emit a debugging log message on these tokens.
        // Note: message are supressed during the first load -- change some lines to see them.
        [/@\s*[a-zA-Z_\$][\w\$]*/, { token: 'annotation', log: 'annotation token: $0' }],

        // numbers
        [/\d*\.\d+([eE][\-+]?\d+)?/, 'number.float'],
        [/0[xX][0-9a-fA-F]+/, 'number.hex'],
        [/0[b][0-1]+/, 'number.bin'],
        [/\d+/, 'number'],

        // // delimiter: after number because of .\d floats
        [/[,.]/, 'delimiter'],
        [/\;+.*/, 'comment', '@push'],

        // strings
        [/"([^"\\]|\\.)*$/, 'string.invalid'],  // non-teminated string
        [/"/, { token: 'string.quote', bracket: '@open', next: '@string' }],

        // characters
        [/'[^\\']'/, 'string'],
        [/(')(@escapes)(')/, ['string', 'string.escape', 'string']],
        [/'/, 'string.invalid'],

      ],

      string: [
        [/[^\\"]+/, 'string'],
        [/@escapes/, 'string.escape'],
        [/\\./, 'string.escape.invalid'],
        [/"/, { token: 'string.quote', bracket: '@close', next: '@pop' }]
      ],

      whitespace: [
        [/[ \t\r\n]+/, 'white'],
        //        [/\/\*/, 'comment', '@comment'],
        //        [/\/\/.*$/, 'comment'],
      ],
    }
  });

  monaco.editor.defineTheme('customVisualTheme', {
    base: 'vs-dark', // can also be vs-dark or hc-black
    inherit: true, // can also be false to completely replace the builtin rules
    rules: [
      { token: 'comment', foreground: '8c8c8c', fontStyle: 'italic' },
      { token: 'register', fontStyle: 'bold' },
    ]
  });

  window.code = monaco.editor.create(document.getElementById('editor'), {
    value: [
      'ADD R0, R0, #1; test comment'
    ].join('\n'),
    language: 'arm',
    theme: 'customVisualTheme',
    renderWhitespace: 'none',
    roundedSelection: false,
    scrollBeyondLastLine: false
  });

  window.setError = function (err, line){
    var startIndex = 0;
    var errorMarker =
      [{
        severity: monaco.Severity.Error,
        startLineNumber: line,
        startColumn: 1,
        endLineNumber: line,
        endColumn: 1000,
        message: err
      }];
    
    var model = monaco.editor.getModels()[0];
    
    monaco.editor.setModelMarkers(model, "arm", errorMarker);
  };

  window.clearError = function(holder){
    var errorMarker = [{}];
    console.log("clear markers");
    var model = monaco.editor.getModels()[0];
    
    monaco.editor.setModelMarkers(model, "arm", errorMarker);
  };

});
