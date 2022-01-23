.feature force_range
.debuginfo +
.setcpu "65c02"
.macpack longbranch
.list on

.feature org_per_seg
.segment "ZEROPAGE"
.zeropage
.org $0000

; Main code
.segment "CODE"
.org $8000

Reset:
    
    CLC
    CLD
    CLI
    CLV

    ; ADC - Immediate
    LDA #$00
    ADC #$00
    CLC
    LDA #$00
    ADC #$01
    CLC
    LDA #$00
    ADC #$FF
    CLC
    LDA #$01
    ADC #$02
    CLC
    LDA #$01
    ADC #$FE
    CLC
    LDA #$FE
    ADC #$02
    CLC
    LDA #$FE
    ADC #$05
    CLC

    SEC
    LDA #$00
    ADC #$00
    SEC
    LDA #$00
    ADC #$01
    SEC
    LDA #$01
    ADC #$00
    SEC
    LDA #$01
    ADC #$02
    SEC
    LDA #$FE
    ADC #$00
    SEC
    LDA #$FE
    ADC #$01
    SEC
    LDA #$FE
    ADC #$03

    CLC
    LDA #$FF
    ADC #$FF
    SEC
    LDA #$FF
    ADC #$FF
    CLC

    ; ADC Zeropage
    LDA #$03
    STA $05
    LDA #$AA
    ADC $05

    LDA #$00
    STA $05
    CLC

    LDA #$03
    STA $08
    LDA #$AA
    LDX #$03
    ADC $05,X

    LDA #$00
    STA $08
    LDX #$00
    CLC

    LDA #$03
    STA $01
    LDA #$AA
    LDX #$03
    ADC $FE,X

    LDA #$00
    STA $01
    LDX #$00
    CLC

    ; ADC Absolute
    LDA #$03
    STA $5005
    LDA #$AA
    ADC $5005

    LDA #$00
    STA $5005
    CLC

    LDA #$03
    STA $5005
    LDA #$AA
    LDX #$03
    ADC $5002,X

    LDA #$00
    STA $5005
    LDX #$00
    CLC

    LDA #$03
    STA $5001
    LDA #$AA
    LDX #$03
    ADC $4FFE,X

    LDA #$00
    STA $5001
    LDX #$00
    CLC

    LDA #$03
    STA $5005
    LDA #$AA
    LDY #$03
    ADC $5002,Y

    LDA #$00
    STA $5005
    LDY #$00
    CLC

    LDA #$03
    STA $5001
    LDA #$AA
    LDY #$03
    ADC $4FFE,Y

    LDA #$00
    STA $5001
    LDY #$00
    CLC

    ; ADC Indirect
    LDA #$03
    STA $5005
    LDA #$05
    STA $05
    LDA #$50
    STA $06
    LDA #$AA
    LDX #$00
    ADC ($05,X)

    LDA #$00
    STA $05
    STA $06
    STA $5005
    LDX #$00
    CLC

    LDA #$03
    STA $5005
    LDA #$05
    STA $05
    LDA #$50
    STA $06
    LDA #$AA
    LDX #$03
    ADC ($02,X)

    LDA #$00
    STA $05
    STA $06
    STA $5005
    LDX #$00
    CLC

    LDA #$03
    STA $5005
    LDA #$05
    STA $05
    LDA #$50
    STA $06
    LDA #$AA
    LDX #$FF
    ADC ($05,X)

    LDA #$00
    STA $05
    STA $06
    STA $5005
    LDX #$00
    CLC

    LDA #$03
    STA $4FFE
    LDA #$FE
    STA $05
    LDA #$4F
    STA $06
    LDA #$AA
    LDY #$00
    ADC ($05),Y

    LDA #$00
    STA $05
    STA $06
    STA $4FFE
    LDY #$00
    CLC

    LDA #$03
    STA $5001
    LDA #$FE
    STA $05
    LDA #$4F
    STA $06
    LDA #$AA
    LDY #$03
    ADC ($05),Y

    LDA #$00
    STA $05
    STA $06
    STA $5001
    LDY #$00
    CLC

    ; AND - Immediate
    LDA #$00
    AND #$00
    CLC
    LDA #$00
    AND #$01
    CLC
    LDA #$00
    AND #$FF
    CLC
    LDA #$01
    AND #$02
    CLC
    LDA #$01
    AND #$FE
    CLC
    LDA #$FE
    AND #$02
    CLC
    LDA #$FE
    AND #$05
    CLC

    SEC
    LDA #$00
    AND #$00
    SEC
    LDA #$00
    AND #$01
    SEC
    LDA #$01
    AND #$00
    SEC
    LDA #$01
    AND #$02
    SEC
    LDA #$FE
    AND #$00
    SEC
    LDA #$FE
    AND #$01
    SEC
    LDA #$FE
    AND #$03

    CLC
    LDA #$FF
    AND #$FF
    SEC
    LDA #$FF
    AND #$FF
    CLC

    ; AND Zeropage
    LDA #$03
    STA $05
    LDA #$AA
    AND $05

    LDA #$00
    STA $05
    CLC

    LDA #$03
    STA $08
    LDA #$AA
    LDX #$03
    AND $05,X

    LDA #$00
    STA $08
    LDX #$00
    CLC

    LDA #$03
    STA $01
    LDA #$AA
    LDX #$03
    AND $FE,X

    LDA #$00
    STA $01
    LDX #$00
    CLC

    ; AND Absolute
    LDA #$03
    STA $5005
    LDA #$AA
    AND $5005

    LDA #$00
    STA $5005
    CLC

    LDA #$03
    STA $5005
    LDA #$AA
    LDX #$03
    AND $5002,X

    LDA #$00
    STA $5005
    LDX #$00
    CLC

    LDA #$03
    STA $5001
    LDA #$AA
    LDX #$03
    AND $4FFE,X

    LDA #$00
    STA $5001
    LDX #$00
    CLC

    LDA #$03
    STA $5005
    LDA #$AA
    LDY #$03
    AND $5002,Y

    LDA #$00
    STA $5005
    LDY #$00
    CLC

    LDA #$03
    STA $5001
    LDA #$AA
    LDY #$03
    AND $4FFE,Y

    LDA #$00
    STA $5001
    LDY #$00
    CLC

    ; AND Indirect
    LDA #$03
    STA $5005
    LDA #$05
    STA $05
    LDA #$50
    STA $06
    LDA #$AA
    LDX #$00
    AND ($05,X)

    LDA #$00
    STA $05
    STA $06
    STA $5005
    LDX #$00
    CLC

    LDA #$03
    STA $5005
    LDA #$05
    STA $05
    LDA #$50
    STA $06
    LDA #$AA
    LDX #$03
    AND ($02,X)

    LDA #$00
    STA $05
    STA $06
    STA $5005
    LDX #$00
    CLC

    LDA #$03
    STA $5005
    LDA #$05
    STA $05
    LDA #$50
    STA $06
    LDA #$AA
    LDX #$FF
    AND ($05,X)

    LDA #$00
    STA $05
    STA $06
    STA $5005
    LDX #$00
    CLC

    LDA #$03
    STA $4FFE
    LDA #$FE
    STA $05
    LDA #$4F
    STA $06
    LDA #$AA
    LDY #$00
    AND ($05),Y

    LDA #$00
    STA $05
    STA $06
    STA $4FFE
    LDY #$00
    CLC

    LDA #$03
    STA $5001
    LDA #$FE
    STA $05
    LDA #$4F
    STA $06
    LDA #$AA
    LDY #$03
    AND ($05),Y

    LDA #$00
    STA $05
    STA $06
    STA $5001
    LDY #$00
    CLC

    ; AND Indirect
    LDA #$03
    STA $5005
    LDA #$05
    STA $05
    LDA #$50
    STA $06
    LDA #$AA
    LDX #$00
    AND ($05,X)

    LDA #$00
    STA $05
    STA $06
    STA $5005
    LDX #$00
    CLC

    LDA #$03
    STA $5005
    LDA #$05
    STA $05
    LDA #$50
    STA $06
    LDA #$AA
    LDX #$03
    AND ($02,X)

    LDA #$00
    STA $05
    STA $06
    STA $5005
    LDX #$00
    CLC

    LDA #$03
    STA $5005
    LDA #$05
    STA $05
    LDA #$50
    STA $06
    LDA #$AA
    LDX #$FF
    AND ($05,X)

    LDA #$00
    STA $05
    STA $06
    STA $5005
    LDX #$00
    CLC

    LDA #$03
    STA $4FFE
    LDA #$FE
    STA $05
    LDA #$4F
    STA $06
    LDA #$AA
    LDY #$00
    AND ($05),Y

    LDA #$00
    STA $05
    STA $06
    STA $4FFE
    LDY #$00
    CLC

    LDA #$03
    STA $5001
    LDA #$FE
    STA $05
    LDA #$4F
    STA $06
    LDA #$AA
    LDY #$03
    AND ($05),Y

    LDA #$00
    STA $05
    STA $06
    STA $5001
    LDY #$00
    CLC

    ; ORA - Immediate
    LDA #$00
    ORA #$00
    CLC
    LDA #$00
    ORA #$01
    CLC
    LDA #$00
    ORA #$FF
    CLC
    LDA #$01
    ORA #$02
    CLC
    LDA #$01
    ORA #$FE
    CLC
    LDA #$FE
    ORA #$02
    CLC
    LDA #$FE
    ORA #$05
    CLC

    SEC
    LDA #$00
    ORA #$00
    SEC
    LDA #$00
    ORA #$01
    SEC
    LDA #$01
    ORA #$00
    SEC
    LDA #$01
    ORA #$02
    SEC
    LDA #$FE
    ORA #$00
    SEC
    LDA #$FE
    ORA #$01
    SEC
    LDA #$FE
    ORA #$03

    CLC
    LDA #$FF
    ORA #$FF
    SEC
    LDA #$FF
    ORA #$FF
    CLC

    ; ORA Zeropage
    LDA #$03
    STA $05
    LDA #$AA
    ORA $05

    LDA #$00
    STA $05
    CLC

    LDA #$03
    STA $08
    LDA #$AA
    LDX #$03
    ORA $05,X

    LDA #$00
    STA $08
    LDX #$00
    CLC

    LDA #$03
    STA $01
    LDA #$AA
    LDX #$03
    ORA $FE,X

    LDA #$00
    STA $01
    LDX #$00
    CLC

    ; ORA Absolute
    LDA #$03
    STA $5005
    LDA #$AA
    ORA $5005

    LDA #$00
    STA $5005
    CLC

    LDA #$03
    STA $5005
    LDA #$AA
    LDX #$03
    ORA $5002,X

    LDA #$00
    STA $5005
    LDX #$00
    CLC

    LDA #$03
    STA $5001
    LDA #$AA
    LDX #$03
    ORA $4FFE,X

    LDA #$00
    STA $5001
    LDX #$00
    CLC

    LDA #$03
    STA $5005
    LDA #$AA
    LDY #$03
    ORA $5002,Y

    LDA #$00
    STA $5005
    LDY #$00
    CLC

    LDA #$03
    STA $5001
    LDA #$AA
    LDY #$03
    ORA $4FFE,Y

    LDA #$00
    STA $5001
    LDY #$00
    CLC

    ; ORA Indirect
    LDA #$03
    STA $5005
    LDA #$05
    STA $05
    LDA #$50
    STA $06
    LDA #$AA
    LDX #$00
    ORA ($05,X)

    LDA #$00
    STA $05
    STA $06
    STA $5005
    LDX #$00
    CLC

    LDA #$03
    STA $5005
    LDA #$05
    STA $05
    LDA #$50
    STA $06
    LDA #$AA
    LDX #$03
    ORA ($02,X)

    LDA #$00
    STA $05
    STA $06
    STA $5005
    LDX #$00
    CLC

    LDA #$03
    STA $5005
    LDA #$05
    STA $05
    LDA #$50
    STA $06
    LDA #$AA
    LDX #$FF
    ORA ($05,X)

    LDA #$00
    STA $05
    STA $06
    STA $5005
    LDX #$00
    CLC

    LDA #$03
    STA $4FFE
    LDA #$FE
    STA $05
    LDA #$4F
    STA $06
    LDA #$AA
    LDY #$00
    ORA ($05),Y

    LDA #$00
    STA $05
    STA $06
    STA $4FFE
    LDY #$00
    CLC

    LDA #$03
    STA $5001
    LDA #$FE
    STA $05
    LDA #$4F
    STA $06
    LDA #$AA
    LDY #$03
    ORA ($05),Y

    LDA #$00
    STA $05
    STA $06
    STA $5001
    LDY #$00
    CLC

    ; ORA Indirect
    LDA #$03
    STA $5005
    LDA #$05
    STA $05
    LDA #$50
    STA $06
    LDA #$AA
    LDX #$00
    ORA ($05,X)

    LDA #$00
    STA $05
    STA $06
    STA $5005
    LDX #$00
    CLC

    LDA #$03
    STA $5005
    LDA #$05
    STA $05
    LDA #$50
    STA $06
    LDA #$AA
    LDX #$03
    ORA ($02,X)

    LDA #$00
    STA $05
    STA $06
    STA $5005
    LDX #$00
    CLC

    LDA #$03
    STA $5005
    LDA #$05
    STA $05
    LDA #$50
    STA $06
    LDA #$AA
    LDX #$FF
    ORA ($05,X)

    LDA #$00
    STA $05
    STA $06
    STA $5005
    LDX #$00
    CLC

    LDA #$03
    STA $4FFE
    LDA #$FE
    STA $05
    LDA #$4F
    STA $06
    LDA #$AA
    LDY #$00
    ORA ($05),Y

    LDA #$00
    STA $05
    STA $06
    STA $4FFE
    LDY #$00
    CLC

    LDA #$03
    STA $5001
    LDA #$FE
    STA $05
    LDA #$4F
    STA $06
    LDA #$AA
    LDY #$03
    ORA ($05),Y

    LDA #$00
    STA $05
    STA $06
    STA $5001
    LDY #$00
    CLC

    ; EOR - Immediate
    LDA #$00
    EOR #$00
    CLC
    LDA #$00
    EOR #$01
    CLC
    LDA #$00
    EOR #$FF
    CLC
    LDA #$01
    EOR #$02
    CLC
    LDA #$01
    EOR #$FE
    CLC
    LDA #$FE
    EOR #$02
    CLC
    LDA #$FE
    EOR #$05
    CLC

    SEC
    LDA #$00
    EOR #$00
    SEC
    LDA #$00
    EOR #$01
    SEC
    LDA #$01
    EOR #$00
    SEC
    LDA #$01
    EOR #$02
    SEC
    LDA #$FE
    EOR #$00
    SEC
    LDA #$FE
    EOR #$01
    SEC
    LDA #$FE
    EOR #$03

    CLC
    LDA #$FF
    EOR #$FF
    SEC
    LDA #$FF
    EOR #$FF
    CLC

    ; EOR Zeropage
    LDA #$03
    STA $05
    LDA #$AA
    EOR $05

    LDA #$00
    STA $05
    CLC

    LDA #$03
    STA $08
    LDA #$AA
    LDX #$03
    EOR $05,X

    LDA #$00
    STA $08
    LDX #$00
    CLC

    LDA #$03
    STA $01
    LDA #$AA
    LDX #$03
    EOR $FE,X

    LDA #$00
    STA $01
    LDX #$00
    CLC

    ; EOR Absolute
    LDA #$03
    STA $5005
    LDA #$AA
    EOR $5005

    LDA #$00
    STA $5005
    CLC

    LDA #$03
    STA $5005
    LDA #$AA
    LDX #$03
    EOR $5002,X

    LDA #$00
    STA $5005
    LDX #$00
    CLC

    LDA #$03
    STA $5001
    LDA #$AA
    LDX #$03
    EOR $4FFE,X

    LDA #$00
    STA $5001
    LDX #$00
    CLC

    LDA #$03
    STA $5005
    LDA #$AA
    LDY #$03
    EOR $5002,Y

    LDA #$00
    STA $5005
    LDY #$00
    CLC

    LDA #$03
    STA $5001
    LDA #$AA
    LDY #$03
    EOR $4FFE,Y

    LDA #$00
    STA $5001
    LDY #$00
    CLC

    ; EOR Indirect
    LDA #$03
    STA $5005
    LDA #$05
    STA $05
    LDA #$50
    STA $06
    LDA #$AA
    LDX #$00
    EOR ($05,X)

    LDA #$00
    STA $05
    STA $06
    STA $5005
    LDX #$00
    CLC

    LDA #$03
    STA $5005
    LDA #$05
    STA $05
    LDA #$50
    STA $06
    LDA #$AA
    LDX #$03
    EOR ($02,X)

    LDA #$00
    STA $05
    STA $06
    STA $5005
    LDX #$00
    CLC

    LDA #$03
    STA $5005
    LDA #$05
    STA $05
    LDA #$50
    STA $06
    LDA #$AA
    LDX #$FF
    EOR ($05,X)

    LDA #$00
    STA $05
    STA $06
    STA $5005
    LDX #$00
    CLC

    LDA #$03
    STA $4FFE
    LDA #$FE
    STA $05
    LDA #$4F
    STA $06
    LDA #$AA
    LDY #$00
    EOR ($05),Y

    LDA #$00
    STA $05
    STA $06
    STA $4FFE
    LDY #$00
    CLC

    LDA #$03
    STA $5001
    LDA #$FE
    STA $05
    LDA #$4F
    STA $06
    LDA #$AA
    LDY #$03
    EOR ($05),Y

    LDA #$00
    STA $05
    STA $06
    STA $5001
    LDY #$00
    CLC

    ; EOR Indirect
    LDA #$03
    STA $5005
    LDA #$05
    STA $05
    LDA #$50
    STA $06
    LDA #$AA
    LDX #$00
    EOR ($05,X)

    LDA #$00
    STA $05
    STA $06
    STA $5005
    LDX #$00
    CLC

    LDA #$03
    STA $5005
    LDA #$05
    STA $05
    LDA #$50
    STA $06
    LDA #$AA
    LDX #$03
    EOR ($02,X)

    LDA #$00
    STA $05
    STA $06
    STA $5005
    LDX #$00
    CLC

    LDA #$03
    STA $5005
    LDA #$05
    STA $05
    LDA #$50
    STA $06
    LDA #$AA
    LDX #$FF
    EOR ($05,X)

    LDA #$00
    STA $05
    STA $06
    STA $5005
    LDX #$00
    CLC

    LDA #$03
    STA $4FFE
    LDA #$FE
    STA $05
    LDA #$4F
    STA $06
    LDA #$AA
    LDY #$00
    EOR ($05),Y

    LDA #$00
    STA $05
    STA $06
    STA $4FFE
    LDY #$00
    CLC

    LDA #$03
    STA $5001
    LDA #$FE
    STA $05
    LDA #$4F
    STA $06
    LDA #$AA
    LDY #$03
    EOR ($05),Y

    LDA #$00
    STA $05
    STA $06
    STA $5001
    LDY #$00
    CLC

    BRK

IRQ:
    RTI

NMI:
    RTI

; Interupt vectors
.segment "VECTORS"
.org $FFFA
	.word	NMI
	.word	Reset
	.word	IRQ
